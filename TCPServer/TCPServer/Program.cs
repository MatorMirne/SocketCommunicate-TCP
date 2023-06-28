using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Linq;
using TCPServer;
using Reservoir;

namespace TCPServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            // 서버 소켓 발행
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 51225);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(endPoint);
            serverSocket.Listen(10); // 추후 3만으로 업그레이드

            // 클라이언트 대기 스레드 실행
            Thread waitClientThread = new Thread(WaitClientThread);
            waitClientThread.Start(serverSocket);

            // exit 입력 시 서버 종료
            string input="";
            while (input != "exit")
            {
                input = Console.ReadLine();
                int threadCount = ThreadPool.ThreadCount;
                Console.WriteLine($"스레드풀의 스레드 수 : {threadCount}");
            }

            // 서버 종료
            serverSocket.Close();
        }

        static void WaitClientThread(object socket)
        {
            Socket serverSocket = socket as Socket;
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();

                // 클라이언트 하나당 스레드풀에서 스레드 하나를 할당
                ThreadPool.QueueUserWorkItem(ClientWork, clientSocket);
            }
        }

        private static void ClientWork(object state)
        {
            Remote remote = RemotePool.AddConnection(state as Socket);

            Run(remote);
        }
        
        
        static void Run(Remote remote)
        {
            while (true)
            {
                if (Receive(remote)) Send(remote);
                else break;
            }
            Close(remote);
        }
        
        static bool Receive(Remote remote)
        {
            int length = remote.socket.Receive(remote.receiveBuffer, 0, remote.receiveBuffer.Length, SocketFlags.None);
            string receiveMessage = System.Text.Encoding.UTF8.GetString(remote.receiveBuffer, 0, length);

            if (receiveMessage == "") return false; // 이렇게 하는게 맞나요?

            remote.count++;
            Console.WriteLine($"수신 : {receiveMessage}");
            return true;
        }

        static void Send(Remote remote)
        {
            int newCount = remote.count + 1;
            string sendMessage = newCount.ToString();
            remote.sendBuffer = System.Text.Encoding.UTF8.GetBytes(sendMessage);
            remote.socket.Send(remote.sendBuffer, 0, sendMessage.Length, SocketFlags.None);
            Console.WriteLine($"발신 : {sendMessage}");
        }

        static void Close(Remote remote)
        {
            remote.socket.Shutdown(SocketShutdown.Both);
            remote.socket.Close();
        }
    }
}
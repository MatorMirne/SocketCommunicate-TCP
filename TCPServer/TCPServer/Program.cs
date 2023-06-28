using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Linq;
using TCPServer;

namespace TCPServer
{
    public static
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
            Remote remote = new Remote(state as Socket);
        }
    }
}
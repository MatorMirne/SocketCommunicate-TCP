using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    partial class  Program
    {
        public static void Main(string[] args)
        {
            // 서버 소켓 발행
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 51225);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(endPoint);
            serverSocket.Listen(10);
            
            // 클라이언트 대기 스레드 실행
            Thread waitClientThread = new Thread(WaitClientThreadAsync);
            waitClientThread.Start(serverSocket);

            // 주 스레드는 입력 처리
            string input = "";
            while (input != "exit")
            {
                if (input == "connect test")
                {
                    RemotePool.remotePool.FindDisconnect();
                }
                input = Console.ReadLine();
                PrintState(input);
            }

            // 서버 종료
            serverSocket.Close();
        }
        
        // 클라이언트 연결 요청을 대기하는 스레드
        static async void WaitClientThreadAsync(object socket)
        {
            Socket serverSocket = socket as Socket;
            while (true)
            {
                Socket clientSocket = await serverSocket.AcceptAsync();
                Remote remote;

                lock (RemotePool.remotePool)
                {
                    RemotePool.remotePool.FindDisconnect();
                    remote = RemotePool.AddConnection(clientSocket as Socket);
                }

                Task.Run(async () => RunAsync(remote));
            }
        }
    }
}
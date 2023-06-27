using System.Net;
using System.Net.Sockets;

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

            // 입력 감지 시 서버 종료
            Console.ReadKey();

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
            Client client = new Client(state as Socket);
        }
    }
}
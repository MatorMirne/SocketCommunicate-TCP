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
                // 연결 미종료된 노드 검색 // 삭제
                // if (input == "connect test")
                // {
                //     RemotePool.remotePool.FindDisconnect();
                // }
                
                input = Console.ReadLine();
                PrintState(input);
            }

            // 서버 종료
            serverSocket.Close();
        }
    }
}
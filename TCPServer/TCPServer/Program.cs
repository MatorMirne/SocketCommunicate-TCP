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
            serverSocket.Listen(10); // 추후 3만으로 업그레이드

            // 클라이언트 대기 스레드 실행
            Thread waitClientThread = new Thread(WaitClientThread);
            waitClientThread.Start(serverSocket);

            // 주 스레드는 입력을 받음
            string input="";
            while (input != "exit")
            {
                input = Console.ReadLine();
                Print(input);
            }

            // 서버 종료
            serverSocket.Close();
        }
    }
}
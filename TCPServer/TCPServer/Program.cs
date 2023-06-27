using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 51225);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(endPoint);
            serverSocket.Listen(10); // 추후 3만으로 업그레이드

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Client client = new Client(clientSocket);
            }

            serverSocket.Close();
        }
    }
}
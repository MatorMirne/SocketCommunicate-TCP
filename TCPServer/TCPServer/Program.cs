using System.Net;
using System.Net.Sockets;

class Program
{
    public static void Main(string[] args)
    {
        Socket serverSocket = SetServer();
        RunServer(serverSocket);
        CloseServer(serverSocket);
    }

    public static Socket SetServer()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 51225);
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        serverSocket.Bind(endPoint);
        serverSocket.Listen(10);

        return serverSocket;
    }
    
    public static void RunServer(Socket serverSocket)
    {
        while (true)
        {
            Socket clientSocket = serverSocket.Accept();

            byte[] receiveBuffer = new byte[1024];
            int length = clientSocket.Receive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None);
            string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
            Console.WriteLine(receiveMessage);

            string sendMessage = (int.Parse(receiveMessage) + 1).ToString();
            byte[] sendBuffer = System.Text.Encoding.UTF8.GetBytes(sendMessage);
            clientSocket.Send(sendBuffer, 0, sendMessage.Length, SocketFlags.None);
            Console.WriteLine(sendMessage);
    
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
    
    public static void CloseServer(Socket serverSocket)
    {
        serverSocket.Shutdown(SocketShutdown.Both);
        serverSocket.Close();
    }
}
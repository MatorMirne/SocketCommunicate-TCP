using System.Net.Mail;

namespace Jesus;
using System.Net.Sockets;
using System.Net;

public class Server : IDisposable
{
    public  Socket socket;

    public Server()
    {
        socket = SetServer();
    }
    
    
    private Socket SetServer()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 51225);
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        serverSocket.Bind(endPoint);
        serverSocket.Listen(10);

        return serverSocket;
    }
    
    public void RunServer()
    {
        while (true)
        {
            Socket clientSocket = socket.Accept();

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

    public void CloseServer()
    {
        Console.WriteLine("서버소켓 반환");
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
    
    public void Dispose()
    {
        Console.WriteLine("Dispose 호출");
        CloseServer();
    }
}
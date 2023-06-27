using System.Net;
using System.Net.Sockets;
namespace TCPClient;

public class Client
{
    byte[] sendBuffer = new byte[1024];
    byte[] receiveBuffer = new byte[1024];
    Socket socket;

    public Client()
    {
        Connect();
        Work();
        Disconnect();
    }

    void Connect()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 51225);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(endPoint);
    }

    void Work()
    {
        for (int i = 0; i < 3; i++)
        {
            string message = i.ToString();
            sendBuffer = System.Text.Encoding.UTF8.GetBytes(message);
            socket.Send(sendBuffer);

            socket.Receive(receiveBuffer);
            string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, receiveBuffer.Length);
            Console.WriteLine(receiveMessage);
        }
    }

    void Disconnect()
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}
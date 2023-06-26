using System.Net;
using System.Net.Sockets;

class Program
{
    public static void Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 51225);

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(endPoint);
        
        string message = "1";
        byte[] sendBuffer = System.Text.Encoding.UTF8.GetBytes(message);
        socket.Send(sendBuffer);
        
        byte[] receiveBuffer = new byte[1024];
        int receiveSize = socket.Receive(receiveBuffer);
        string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, receiveSize);
        
        Console.WriteLine(receiveMessage);
        socket.Close();
    }
}
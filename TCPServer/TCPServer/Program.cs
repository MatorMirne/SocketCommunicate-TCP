using System.Net;
using System.Net.Sockets;

class Program
{
    public static void Main(string[] args)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 51225);
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        serverSocket.Bind(endPoint);
        serverSocket.Listen(10); // 추후 3만으로 업그레이드
        Socket clientSocket = serverSocket.Accept(); // 저장된 소켓 정보를 반환
        
        

        byte[] receiveBuffer = new byte[1024];
        int length = clientSocket.Receive(receiveBuffer, 0, 1024, SocketFlags.None);
        string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
        Console.WriteLine(receiveMessage);
        
        string sendMessage = (int.Parse(receiveMessage) + 1).ToString();
        byte[] sendBuffer = System.Text.Encoding.UTF8.GetBytes(sendMessage);
        clientSocket.Send(sendBuffer, 0, sendMessage.Length, SocketFlags.None);
        Console.WriteLine(sendMessage);
        
        clientSocket.Close();
        serverSocket.Close();
    }
}
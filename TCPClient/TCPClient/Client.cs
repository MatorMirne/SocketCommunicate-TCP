using System.Net;
using System.Net.Sockets;

namespace TCPClient;

public class ClientCounter
{
    public static ClientCounter clientCounter = new ClientCounter();
    public int count = 0;
}

public class Client
{
    public static int count = 0;

    byte[] sendBuffer = new byte[1024];
    byte[] receiveBuffer = new byte[1024];
    Socket socket;

    public Client()
    {
        Connect();
        WorkAsync();
    }

    void Connect()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 51225);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(endPoint);
    }

    async void WorkAsync()
    {
        for(int i=0; i<3; i++)
        {
            string message = "데이터 요청";
            sendBuffer = System.Text.Encoding.UTF8.GetBytes(message);
            
            // 비동기로 던지고
            Task.Run(()=>socket.SendAsync(sendBuffer));

            // 받는거는 기다리고
            var length = await socket.ReceiveAsync(receiveBuffer);
            
            string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
            Console.WriteLine($"{i} 수신 : {receiveMessage}");
        }

        lock (ClientCounter.clientCounter)
        {
            ClientCounter.clientCounter.count++;
            if (ClientCounter.clientCounter.count == 100)
            {
                Console.WriteLine($"{ClientCounter.clientCounter.count}명 모두 통신 완료! 걸린 시간 : {MyStopWatch.Stop()}");
            }
        }
    }

    public void Disconnect()
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}
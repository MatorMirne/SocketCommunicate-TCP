using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text.Json;

using Protocol;

namespace TCPClient;

public class ClientCounter
{
    public static ClientCounter clientCounter = new ClientCounter();
    public int count = 0;
}

public class Client
{
    public int Id { get; set; }
    
    public static int count = 0;

    byte[] sendBuffer = new byte[1024];
    byte[] receiveBuffer = new byte[1024];
    Socket socket;

    public Client()
    {
        Connect();
        WorkAsync();
    }
    
    public Client(int id)
    {
        Id = id;
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
        var input = "";

        while (string.Equals(input, "exit") == false)
        {
            try
            {
                string json;

                if (false) // for message
                {
                    var request = new MessageRequest();
                    request.Message = "msg";
                    json = JsonSerializer.Serialize(request);
                }

                if (true)
                {
                    var request = new HandshakeRequest();
                    request.ClientId = Id; // 다운캐스팅
                    json = JsonSerializer.Serialize(request);
                }

                Console.WriteLine($"request:{json}");

                sendBuffer = System.Text.Encoding.UTF8.GetBytes(json);

                // 비동기로 던지고
                await Task.Run( () => socket.SendAsync(sendBuffer));

                int length = 0;
                try
                {
                    length = await socket.ReceiveAsync(receiveBuffer);
                }
                catch (SocketException e)
                {
                    if (e.ErrorCode == 89) break; // 듣기 중단 (들을거 없음)
                    if (e.ErrorCode == 57) break; // 듣기 중단 (연결 종료)
                    else throw;
                }
                catch (ObjectDisposedException e)
                {
                    if (e.ObjectName == "System.Net.Sockets.Socket") break; // 소켓 반납 이후에는 듣지 않음
                    else throw;
                }

                string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
                Console.WriteLine($"수신 : {receiveMessage}");
            }
            catch
            {
                Console.WriteLine($"ID:{Id} 에서 에러 발생");
            }
        }

        lock (ClientCounter.clientCounter)
        {
            ClientCounter.clientCounter.count++;
        }
    }

    public void Disconnect()
    {
        socket.Disconnect(true);
        socket.Close(); 
    }
    
    ~Client()
    {
        Disconnect();
        Console.WriteLine("소멸자 호출");
    }
}
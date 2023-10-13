using System.Net.Sockets;
using System.Text.Json;
using Protocol;

namespace TCPServer;

public partial class Program
{
    static async Task RunAsync(Remote remote)
    {
        while (true)
        {
            var (isSuccess, response)= await ReceiveAsync(remote);
            if (isSuccess) Send(remote, (ProtocolResponse)response);
            else break;
        }
        Close(remote);
    }
        
    static async Task<(bool, object)> ReceiveAsync(Remote remote)
    {
        int length = await remote.socket.ReceiveAsync(remote.receiveBuffer, SocketFlags.None);
        string receiveMessage = System.Text.Encoding.UTF8.GetString(remote.receiveBuffer, 0, length);

        if (receiveMessage == "") 
            return (false, null);
        if(remote.socket.Connected == false)
            return (false, null);

        // remote.count++;
        Console.WriteLine($"수신 : {receiveMessage}");

        var response = new MessageResponse();
        response.Message = "response message";
        response.Result = Result.Success;
        
        return (true, response);
    }

    static void Send(Remote remote, ProtocolResponse response)
    {
        string json = JsonSerializer.Serialize(response);
        remote.sendBuffer = System.Text.Encoding.UTF8.GetBytes(json);
        remote.socket.Send(remote.sendBuffer, 0, json.Length, SocketFlags.None);
    }

    static void Close(Remote remote)
    {
        remote.socket.Shutdown(SocketShutdown.Both);
        remote.socket.Close();
        remote.socket = null;
            
        // [순서 중요] 메모리를 해제한 후 socket을 제거해야 합니다.
        RemotePool.RemoveConnection(remote);
    }
}
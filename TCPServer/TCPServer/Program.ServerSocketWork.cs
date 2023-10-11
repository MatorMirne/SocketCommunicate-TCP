using System.Net.Sockets;
namespace TCPServer;

public partial class Program
{
    static async Task RunAsync(Remote remote)
    {
        while (true)
        {
            if (await ReceiveAsync(remote)) Send(remote);
            else break;
        }
        Close(remote);
    }
        
    static async Task<bool> ReceiveAsync(Remote remote)
    {
        int length = await remote.socket.ReceiveAsync(remote.receiveBuffer, SocketFlags.None);
        string receiveMessage = System.Text.Encoding.UTF8.GetString(remote.receiveBuffer, 0, length);

        if (receiveMessage == "") 
            return false; // 이렇게 하는게 맞나요?
        if(remote.socket.Connected == false)
            return false; 

        remote.count++;
        // Console.WriteLine($"수신 : {receiveMessage}");
        return true;
    }

    static void Send(Remote remote)
    {
        string sendMessage = remote.count.ToString();
        remote.sendBuffer = System.Text.Encoding.UTF8.GetBytes(sendMessage);
        remote.socket.Send(remote.sendBuffer, 0, sendMessage.Length, SocketFlags.None);
        // Console.WriteLine($"발신 : {sendMessage}");
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
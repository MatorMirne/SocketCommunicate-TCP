using System.Net.Sockets;
namespace TCPServer;

public partial class Program
{
    // 클라이언트 연결 요청을 대기하는 스레드
    static async void WaitClientThreadAsync(object socket)
    {
        Socket serverSocket = socket as Socket;
        while (true)
        {
            Socket clientSocket = await serverSocket.AcceptAsync();
            await ClientWorkAsync(clientSocket);
        }
    }

    private static async Task ClientWorkAsync(object clientSocket)
    {
        Remote remote;
        
        lock (RemotePool.remotePool)
        {
            RemotePool.remotePool.FindDisconnect();
            remote = RemotePool.AddConnection(clientSocket as Socket);
        }

        Task.Run(async () => RunAsync(remote));
    }
}
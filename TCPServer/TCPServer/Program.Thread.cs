using System.Net.Sockets;
namespace TCPServer;

public partial class Program
{
    /// <summary>
    /// 클라이언트 연결 요청을 대기하는 스레드
    /// </summary>
    static async void WaitClientThreadAsync(object socket)
    {
        Socket serverSocket = socket as Socket;
        while (true)
        {
            Socket clientSocket = serverSocket.Accept();
            await ClientWorkAsync(clientSocket);
        }
    }

    private static async Task ClientWorkAsync(object clientSocket)
    {
        Remote remote;
        
        lock (RemotePool.remotePool)
        {
            remote = RemotePool.AddConnection(clientSocket as Socket);
        }

        Task.Run(() => RunAsync(remote));
    }
}
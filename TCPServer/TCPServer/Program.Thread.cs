using System.Net.Sockets;
namespace TCPServer;

public partial class Program
{
    /// <summary>
    /// 클라이언트 연결 요청을 대기하는 스레드
    /// </summary>
    static void WaitClientThread(object socket)
    {
        Socket serverSocket = socket as Socket;
        while (true)
        {
            Socket clientSocket = serverSocket.Accept();

            // 클라이언트 하나당 스레드풀에서 스레드 하나를 할당
            ThreadPool.QueueUserWorkItem(ClientWork, clientSocket);
        }
    }

    /// <summary>
    /// 각 클라이언트마다 하나씩 할당되는 스레드
    /// </summary>
    private static void ClientWork(object state)
    {
        Remote remote = RemotePool.AddConnection(state as Socket);

        Run(remote);
    }
}
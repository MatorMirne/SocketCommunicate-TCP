namespace TCPServer;
using Reservoir;
using System.Net.Sockets;

public class RemotePool
{
    public static RemotePool remotePool = new RemotePool();
    
    private Pool<Remote> pool;
    private NodeList<Remote> list;
    
    public RemotePool()
    {
        pool = new Pool<Remote>();
        list = new NodeList<Remote>();
    }

    /// <summary>
    /// Remote 객체를 Pool에서 할당받고 클라이언트 소켓으로 초가화합니다. 
    /// </summary>
    public static Remote AddConnection(Socket socket)
    {
        Remote remote = remotePool.pool.Allocate();
        
        remote.ID = 1; // ID 발급 알고리즘 필요
        remote.socket = socket;

        remote.count = 0;
        
        remotePool.list.Add(remote);

        return remote;
    }
    
    /// <summary>
    /// Remote 객체를 Pool에 반환합니다.
    /// </summary>
    public static void RemoveConnection(Remote remote)
    {
        Pool.Free(remote);
    }
}
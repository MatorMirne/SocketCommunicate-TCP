using System.Runtime.CompilerServices;

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
        remotePool.list.Remove(remote); // 사용 중 리스트에서 제거
        Pool.Free(remote); // 풀로 오브젝트 반환
    }
    
    /// <summary>
    /// 현재 사용중인 오브젝트 수를 반환합니다.
    /// </summary>
    public int CountActive()
    {
        return remotePool.list.Count;
    }
    
    /// <summary>
    /// 현재 풀에서 놀고 있는 오브젝트 수를 반환합니다.
    /// </summary>
    public int CountAvailable()
    {
        return remotePool.pool.freeList.Count;
    }
    
    public static void PrintState()
    {
        Console.WriteLine($"Conncect:{remotePool.CountActive()} | Available:{remotePool.CountAvailable()}");
    }
}
using System.Runtime.CompilerServices;

namespace TCPServer;
using Reservoir;
using System.Net.Sockets;

public class RemotePool
{
    public static RemotePool remotePool = new RemotePool();
    public static int idCount = 1;

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
        Remote remote;

        lock (remotePool)
        {
            remote = remotePool.pool.Allocate();
            remotePool.list.Add(remote);
        }

        remote.ID = idCount++;
        remote.socket = socket;
        return remote;
    }

    /// <summary>
    /// Remote 객체를 Pool에 반환합니다.
    /// </summary>
    public static void RemoveConnection(Remote remote)
    {
        lock (remotePool)
        {
            // Console.WriteLine($"{remote.ID} 오브젝트 반환");
            remotePool.list.Remove(remote); // 사용 중 리스트에서 제거
            Pool.Free(remote); // 풀로 오브젝트 반환
            PrintState();
        }
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

    public void FindDisconnect()
    {
        var disconnectedRemotes = new List<Remote>();
        foreach (var remote in list)
        {
            if (remote.socket.Connected == false)
            {
                disconnectedRemotes.Add(remote);
            }
        }
        
        foreach (var remote in disconnectedRemotes)
        {
            RemoveConnection(remote);
        }
    }
}
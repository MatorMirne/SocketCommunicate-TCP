namespace TCPServer;

public partial class Program 
{
    static void Print(string input)
    {
        switch (input)
        {
            case "connection":
                // 현재 연결된 클라이언트 ID 출력
                break;    
                
            default:
                RemotePool.PrintState();
                break;
        }

    }
}
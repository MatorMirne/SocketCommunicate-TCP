using System.Diagnostics;
using System.Runtime.CompilerServices;
using TCPClient;

class Program
{
    private static List<Client> clients = new List<Client>();
    
    public static void Main(string[] args)
    {
        // MyStopWatch.Start();

        Process.GetCurrentProcess().Exited += OnCancelKeyPress;
        Console.CancelKeyPress += OnCancelKeyPress;
        
        for (int i = 0; i < 1; i++)
        {
            Task.Run(()=>WorkAsync());
        }

        string input = "";
        while (input != "exit")
        {
            input = Console.ReadLine();
            int threadCount = ThreadPool.ThreadCount;
            // Console.WriteLine($"스레드풀의 스레드 수 : {threadCount}");
            // Console.WriteLine($"통신 완료 클라이언트 수 : {ClientCounter.clientCounter.count}");
        }
        
        Console.CancelKeyPress -= OnCancelKeyPress;

        foreach (var client in clients)
        {
            client.Disconnect();
            Console.WriteLine("클라이언트 종료");
        }
    }

    public static async Task WorkAsync()
    {
        var client = new Client();
        lock (clients)
        {
            clients.Add(client);
        }
    }
    
    private static void OnCancelKeyPress(object sender, EventArgs e)
    {
        Console.WriteLine("클라이언트 종료");
        int cnt = 0;
        foreach (var client in clients)
        {
            cnt++;
            Console.WriteLine($"클라이언트 종료 {cnt}");
            client.Disconnect();
        }
    }
}
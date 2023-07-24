using TCPClient;

class Program
{
    private static List<Client> clients = new List<Client>();
    
    public static void Main(string[] args)
    {
        MyStopWatch.Start();
        
        for (int i = 0; i < 100; i++)
        {
            ThreadPool.QueueUserWorkItem(Work, null);
        }

        string input = "";
        while (input != "exit")
        {
            input = Console.ReadLine();
            int threadCount = ThreadPool.ThreadCount;
            Console.WriteLine($"스레드풀의 스레드 수 : {threadCount}");
        }

        // 입력 감지까지 대기

        foreach (var client in clients)
        {
            client.Disconnect();
        }
    }

    public static void Work(object state)
    {
        clients.Add(new Client());
    }
}
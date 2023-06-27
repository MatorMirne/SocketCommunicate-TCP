using TCPClient;

class Program
{
    public static void Main(string[] args)
    {
        for (int i = 0; i < 10; i++)
        {
            ThreadPool.QueueUserWorkItem(Work, null);
        }

        Thread.Sleep(1000);
        
        int threadCount = ThreadPool.ThreadCount;
        Console.WriteLine($"스레드풀의 스레드 수 : {threadCount}");

        // 입력 감지까지 대기
        Console.ReadKey();
    }

    public static void Work(object state)
    {
        Client client = new Client();
    }
}
using TCPClient;

class Program
{
    public static void Main(string[] args)
    {
        for (int i = 0; i < 10; i++)
        {
            ThreadPool.QueueUserWorkItem(Work, true);
        }

        // 입력 감지까지 대기
        Console.ReadKey();
    }

    public static void Work(object state)
    {
        new Client();
    }
}
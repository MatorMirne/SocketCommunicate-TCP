﻿using TCPClient;

class Program
{
    private static List<Client> clients = new List<Client>();
    
    public static void Main(string[] args)
    {
        for (int i = 0; i < 100; i++)
        {
            ThreadPool.QueueUserWorkItem(Work, null);
        }
        
        int threadCount = ThreadPool.ThreadCount;
        Console.WriteLine($"스레드풀의 스레드 수 : {threadCount}");

        // 입력 감지까지 대기
        Console.ReadKey();

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
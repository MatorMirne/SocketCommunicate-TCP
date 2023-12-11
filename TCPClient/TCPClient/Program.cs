using System.Diagnostics;
using System.Runtime.CompilerServices;
using TCPClient;

class Program
{
	private static List<Client> clients = new List<Client>();

	public static async Task Main(string[] args)
	{
		// MyStopWatch.Start();

		Process.GetCurrentProcess().Exited += OnCancelKeyPress;
		Console.CancelKeyPress += OnCancelKeyPress;

		for (int i = 0; i < 30000; i++)
		{
			await CreateClient(i);
			await Task.Delay(1);
		}

		string input = "";
		while (input != "exit")
		{
			input = Console.ReadLine();

			// for log thread count
			// int threadCount = ThreadPool.ThreadCount;
			// Console.WriteLine($"스레드풀의 스레드 수 : {threadCount}");
			// Console.WriteLine($"통신 완료 클라이언트 수 : {ClientCounter.clientCounter.count}");

			switch (input)
			{
				case "more":
					Console.Write($"추가할 클라이언트 수 :");
					var clientCount = Console.ReadLine();
					try
					{
						var count = int.Parse(clientCount);
						if (count > 1000) count = 1000;
						for (int i = 0; i < count; i++)
						{
							Task.Run(() => CreateClient(i));
						}
					}
					catch
					{
						// ignore
					}

					break;
			}
		}

		Console.CancelKeyPress -= OnCancelKeyPress;

		foreach (var client in clients)
		{
			client.Disconnect();
			Console.WriteLine("클라이언트 종료");
		}
	}

	public static async Task CreateClient(int id)
	{
		var client = new Client(id);
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
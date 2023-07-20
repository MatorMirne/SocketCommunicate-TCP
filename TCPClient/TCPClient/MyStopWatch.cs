using System.Diagnostics;

namespace TCPClient;

public class MyStopWatch
{
    public static MyStopWatch myStopWatch = new MyStopWatch();

    private Stopwatch stopwatch = new Stopwatch();
    private bool isRunning = false;

    public static void Start()
    {
        if (myStopWatch.isRunning)
        {
            Console.WriteLine("싱글톤 스톱워치가 이미 실행중입니다. 나중에 사용하세용. 아니면 싱글톤으로 하지 말던가요~");
            return;
        }

        myStopWatch.isRunning = true;
        myStopWatch.stopwatch.Start();
    }

    public static TimeSpan Stop()
    {
        if (!myStopWatch.isRunning)
        {
            Console.WriteLine("싱글톤 스톱워치가 실행중이 아닙니다. 실행 후 사용하세요.");
            return default;
        }

        myStopWatch.stopwatch.Stop();
        myStopWatch.isRunning = false;

        return myStopWatch.stopwatch.Elapsed;
    }
}
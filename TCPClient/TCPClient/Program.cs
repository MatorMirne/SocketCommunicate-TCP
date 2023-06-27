using TCPClient;

class Program
{
    public static void Main(string[] args)
    {
        for (int i = 0; i < 10; i++)
        {
            new Client();
        }
    }
}
namespace Jesus
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (Server server = new())
            {
                server.RunServer();
            }
        }
    }
}


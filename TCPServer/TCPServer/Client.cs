
using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    public class Client : IDisposable
    {
        Socket socket;
        byte[] receiveBuffer = new byte[1024];
        byte[] sendBuffer = new byte[1024];

        public Client(Socket socket)
        {
            this.socket = socket;
            Run();
        }

        void Run()
        {
            while (true)
            {
                if (Receive())
                {
                    Send();
                }
                else break;   
                
            }
        }

        bool Receive()
        {
            int length = socket.Receive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None);
            string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);

            if (receiveMessage == "") return false;
            
            Console.WriteLine(receiveMessage);
            return true;
        }

        void Send()
        {
            string sendMessage = "send message";
            sendBuffer = System.Text.Encoding.UTF8.GetBytes(sendMessage);
            socket.Send(sendBuffer, 0, sendMessage.Length, SocketFlags.None);
            Console.WriteLine(sendMessage);
        }

        public void Dispose()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
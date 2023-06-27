
using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    public class Client
    {
        Socket socket;
        byte[] receiveBuffer = new byte[1024];
        byte[] sendBuffer = new byte[1024];
        int count;

        public Client(Socket socket)
        {
            this.socket = socket;
            Run();
            Close();
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

            if (receiveMessage == "") return false; // 이렇게 하는게 맞나요?
            
            Console.WriteLine($"수신 : {receiveMessage}");
            count = Int32.Parse(receiveMessage);
            return true;
        }

        void Send()
        {
            int newCount = count + 1;
            string sendMessage = newCount.ToString();
            sendBuffer = System.Text.Encoding.UTF8.GetBytes(sendMessage);
            socket.Send(sendBuffer, 0, sendMessage.Length, SocketFlags.None);
            Console.WriteLine($"발신 : {sendMessage}");
        }

        void Close()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}

using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    public class Client : IDisposable
    {
        Socket socket;

        public Client(Socket socket)
        {
            this.socket = socket;
            
            
            
            
            
            // 
        }

        void Receive()
        {
            byte[] receiveBuffer = new byte[1024];
            int length = socket.Receive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None);
            string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
            Console.WriteLine(receiveMessage);
        }

        void Send()
        {
            string sendMessage = "send message";
            byte[] sendBuffer = System.Text.Encoding.UTF8.GetBytes(sendMessage);
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

using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    public class Remote
    {
        public static int idCount;

        void Reset()
        {
            // Called when the object is put back in the pool
            ID = 0;
            socket = null;
            Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
            Array.Clear(sendBuffer, 0, sendBuffer.Length);
        }

        public int ID;
        public Socket socket;
        public byte[] receiveBuffer = new byte[1024];
        public byte[] sendBuffer = new byte[1024];
        
    }
}

using System.Net;
using System.Net.Sockets;
using Reservoir;

namespace TCPServer
{
    public class Remote : IPoolable<Remote>
    {
        #region IPoolable Members
        
        Pool<Remote> IPoolable<Remote>.Pool { get; set; }
        Remote INode<Remote>.Next { get; set; }
        Remote INode<Remote>.Previous { get; set; }
        NodeList<Remote> INode<Remote>.List { get; set; }
        
        void IPoolable<Remote>.Initialize()
        {
            Console.WriteLine("오브젝트 할당");
        }
        
        void IPoolable<Remote>.Reset()
        {
            // Called when the object is put back in the pool
            ID = 0;
            socket = null;
            Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
            Array.Clear(sendBuffer, 0, sendBuffer.Length);
            count = 0;
        }

        public int ID;
        public Socket socket;
        public byte[] receiveBuffer = new byte[1024];
        public byte[] sendBuffer = new byte[1024];
        public int count;
        
        #endregion
    }
}

using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    public class Remote
    {
        int ID;
        Socket socket;
        byte[] receiveBuffer = new byte[1024];
        byte[] sendBuffer = new byte[1024];
        
        int count;

        public Remote(Socket socket)
        {
            this.socket = socket;
            Run();
            Close();
        }
        
        /*
         * 오브젝트 풀링을 위한 메서드
         * 오브젝트가 다 쓰이고 난 뒤 이 메서드를 호출하여 오브젝트를 초기화
         */
        public void Reset()
        {
            this.ID = 0;
            this.socket = null;
            this.receiveBuffer = null;
            this.sendBuffer = null;

            this.count = 0;

            // 버퍼의 Position도 초기화가 필요할 것 같지만,
            // 어떻게 해야 하는지 모르겠고 byte[] 자체를 초기화하는 것이 맞는지도 모르겠습니다.
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
            
            //Console.WriteLine($"수신 : {receiveMessage}");
            count = Int32.Parse(receiveMessage);
            return true;
        }

        void Send()
        {
            int newCount = count + 1;
            string sendMessage = newCount.ToString();
            sendBuffer = System.Text.Encoding.UTF8.GetBytes(sendMessage);
            socket.Send(sendBuffer, 0, sendMessage.Length, SocketFlags.None);
            //Console.WriteLine($"발신 : {sendMessage}");
        }

        void Close()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
﻿using System.Net;
using System.Net.Sockets;

class Program
{
    public static void Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 51225);

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(endPoint);
        
        byte[] sendBuffer = new byte[1024];
        byte[] receiveBuffer = new byte[1024];

        for (int i = 0; i < 3; i++)
        {
            string message = i.ToString();
            sendBuffer = System.Text.Encoding.UTF8.GetBytes(message);
            socket.Send(sendBuffer);

            socket.Receive(receiveBuffer);
            string receiveMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, receiveBuffer.Length);
            Console.WriteLine(receiveMessage);
        }

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}
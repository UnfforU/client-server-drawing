using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class MyClient
    {
        public static int port = 8005;
        public static string address = "127.0.0.1";
        private IPEndPoint ipPoint;
        private Socket socket;

        public bool isConnected;

        public void CreateSocket()
        {
            ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            socket.Connect(ipPoint);
            isConnected = true;
        }

        public void SendMessage(byte[] data)
        {
            socket.Send(data);
        }

        public void CloseSocket()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            isConnected = false;
        }
    }
}

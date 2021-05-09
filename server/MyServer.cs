using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace server
{
    class MyServer
    {
        public static int port = 8005;
        public static string address = "127.0.0.1";
        private IPEndPoint ipPoint;
        public Socket listenSocket;
        private Socket handler;

        public bool isConnected = false;

        public void CreateSocket()
        {
            ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void BindAndListen()
        {
            listenSocket.Bind(ipPoint);
            listenSocket.Listen(10);
            isConnected = true;
        }

        public void Disconnect()
        {
            if (!isConnected)
            {
                MessageBox.Show("Соединение не было установлено, чтобы его прерывать");
                return;
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }


        private int[] DecryptData(byte[] input)
        {
            {
                int[] result = new int[4];
                int buffInd = 0;

                for (int i = 0; i < 4; i++)
                {
                    byte[] buffnumb = new byte[4];
                    for (int k = 0; k < 4; k++)
                    {
                        buffnumb[k] = input[k + buffInd];
                    }
                    result[i] = BitConverter.ToInt32(buffnumb, 0);
                    buffInd += 4;
                }

                return result;
            }
        }
    }
}

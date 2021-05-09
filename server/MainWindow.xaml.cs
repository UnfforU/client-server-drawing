using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Point firstPoint;
        static Point secondPoint;

        private readonly MyServer server = new MyServer();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void DrawLine(int[] data)
        {
            SetData(data);

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                MyCanvas.Children.Add(new Line
                {
                    X1 = firstPoint.X,
                    Y1 = firstPoint.Y,
                    X2 = secondPoint.X,
                    Y2 = secondPoint.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeThickness = 1,
                    Stroke = Brushes.Black
                });
            });
        }

        private void Listen()
        {
            try
            {
                {
                    Socket handler = server.listenSocket.Accept();

                    while (true)
                    {
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0;
                        byte[] data = new byte[16];

                        do
                        {
                            bytes = handler.Receive(data);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (bytes < 16);

                        int[] result = GetArrIntFromBytes(data);

                        DrawLine(result);
                    }

                    int[] GetArrIntFromBytes(byte[] input)
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void SetData(int[] input)
        {
            firstPoint.X = input[0];
            firstPoint.Y = input[1];
            secondPoint.X = input[2];
            secondPoint.Y = input[3];
        }

        private void ListenButton_Click(object sender, RoutedEventArgs e)
        {          
            try
            {
                server.CreateSocket();
                server.BindAndListen();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            Thread thread = new Thread(Listen);
            thread.Start();
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                server.Disconnect();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}

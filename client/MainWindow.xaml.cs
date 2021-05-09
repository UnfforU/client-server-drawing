using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Point lastPoint;
        static Point currPoint;
        static bool firstTime = true;

        private readonly MyClient client = new MyClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                if (!client.isConnected)
                {
                    MessageBox.Show("Соединение не установлено");
                    return;
                }

                if (firstTime)
                {
                    lastPoint.X = e.GetPosition(MyCanvas).X;
                    lastPoint.Y = e.GetPosition(MyCanvas).Y;
                    firstTime = false;
                    return;
                }

                currPoint = e.GetPosition(MyCanvas);

                MyCanvas.Children.Add(new Line
                {
                    X1 = lastPoint.X,
                    Y1 = lastPoint.Y,
                    X2 = currPoint.X,
                    Y2 = currPoint.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeThickness = 1,
                    Stroke = Brushes.Black
                });

                
                client.SendMessage(GetArrBytesFromInt(lastPoint, currPoint));

                lastPoint.X = e.GetPosition(MyCanvas).X;
                lastPoint.Y = e.GetPosition(MyCanvas).Y;
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.CreateSocket();
                client.Connect();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.CloseSocket();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private byte[] GetArrBytesFromInt(Point firstPoint, Point secondPoint)
        {
            byte[] result = new byte[16];
            int resultInd = 0;

            int[] currInt = new int[4];

            currInt[0] = (int)firstPoint.X;
            currInt[1] = (int)firstPoint.Y;
            currInt[2] = (int)secondPoint.X;
            currInt[3] = (int)secondPoint.Y;

            foreach (int i in currInt)
            {
                byte[] buff = BitConverter.GetBytes(i);
                foreach (byte k in buff)
                {
                    result[resultInd] = k;
                    resultInd++;
                }
            }
            return result;
        }
    }
}



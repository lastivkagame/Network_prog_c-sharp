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

namespace task_1_client_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        struct TransferObject
        {
            public Socket socket { get; set; }

            public byte[] Buffer { get; set; }

            public static readonly int size = 1024;
        }

        private static readonly int port = 2020;
        private static IPAddress ip;
        public static string toserver;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            toserver = mainindextext.Text;
            res_list.Text = "We start searching strets ...";
            StartClient();
            res_list.Text = "We have resalt!";
        }

        private static void StartClient()
        {
            var client = new Socket(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].AddressFamily,
                                        SocketType.Stream, ProtocolType.Tcp);

            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            try
            {
                client.BeginConnect(new IPEndPoint(ip, port), ConnectCallback, client);
            }
            catch (SocketException exception)
            {
                Console.WriteLine(exception.Message);
            }

        }


        private static void ConnectCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState as Socket;
            client.EndConnect(ar);

            TransferObject data = new TransferObject();
            data.Buffer = new byte[TransferObject.size];
            data.socket = client;

            var message = toserver;
            client.BeginSend(Encoding.UTF8.GetBytes(message), 0, message.Length, SocketFlags.None, SendCallback, data);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            var data = (TransferObject)ar.AsyncState;
            var count = data.socket.EndSend(ar);
            data.Buffer = new byte[TransferObject.size];
            data.socket.BeginReceive(data.Buffer, 0, TransferObject.size, SocketFlags.None, ReceiveCallback, data);
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            var data = (TransferObject)ar.AsyncState;
            var count = data.socket.EndReceive(ar);
            var message = Encoding.UTF8.GetString(data.Buffer, 0, count);

            MessageBox.Show(message, "Resalt", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }

    }
}

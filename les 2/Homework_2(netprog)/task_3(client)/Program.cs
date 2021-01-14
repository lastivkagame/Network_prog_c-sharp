using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace task_3_client_
{
    struct TransferObject
    {
        public Socket socket { get; set; }

        public byte[] Buffer { get; set; }

        public static readonly int size = 1024;
    }

    class Program
    {
        private static readonly int port = 8080;
        private static IPAddress ip;

        static void Main(string[] args)
        {
            Console.Title = "Client";
            StartClient();
            Console.ReadLine();
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

            var message = $"Hello from client";//{client.LocalEndPoint}";
            client.BeginSend(Encoding.UTF8.GetBytes(message), 0, message.Length, SocketFlags.None, SendCallback, data);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            var data = (TransferObject)ar.AsyncState;
            var count = data.socket.EndSend(ar);

            Console.WriteLine("->Message was send to server(lenth: {0} bytes)", count);
            
            data.Buffer = new byte[TransferObject.size];
            

            data.socket.BeginReceive(data.Buffer, 0, TransferObject.size, SocketFlags.None, ReceiveCallback, data);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            var data = (TransferObject)ar.AsyncState;
            var count = data.socket.EndReceive(ar);
            var message = Encoding.UTF8.GetString(data.Buffer, 0, count);
            Console.WriteLine("At " + DateTime.Now.ToShortTimeString() + " was receive stroke:  " + message);
        }
    }
}

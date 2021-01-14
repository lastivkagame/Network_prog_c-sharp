using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace task_4_client_
{
    class Program
    {
        struct TransferObject
        {
            public Socket socket { get; set; }

            public byte[] Buffer { get; set; }

            public static readonly int size = 1024;
        }

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
                do
                {
                    Console.Clear();
                    Console.WriteLine("Please choose: ");
                    Console.WriteLine("1.Date");
                    Console.WriteLine("2.Time");
                    Console.WriteLine("3.Exit");
                    Console.Write("Answer: ");
                    messagenumber = Console.ReadLine();
                } while (messagenumber != "1" && messagenumber != "2" && messagenumber != "3");

                if (messagenumber != "3")
                {
                    client.BeginConnect(new IPEndPoint(ip, port), ConnectCallback, client);
                }
                else
                {
                    Console.WriteLine("Bue! Have anice day)");
                }

            }
            catch (SocketException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static string messagenumber = "";

        private static void ConnectCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState as Socket;
            client.EndConnect(ar);
            //string mess = "Hello Everyone!";

            TransferObject data = new TransferObject();
            data.Buffer = new byte[TransferObject.size];
            data.socket = client;

            client.BeginSend(Encoding.UTF8.GetBytes(messagenumber), 0, messagenumber.Length, SocketFlags.None, SendCallback, data);

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

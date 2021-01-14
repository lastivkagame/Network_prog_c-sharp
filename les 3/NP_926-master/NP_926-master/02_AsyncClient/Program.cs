
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _02_AsyncClient
{
    struct TransferObject
    {
        public Socket Socket { get; set; }
        public byte[] Buffer { get; set; }
        public static readonly int size = 1024;
    }
    class Program
    {
        private static readonly int port = 2020;
        private static IPAddress ip;
        static void Main(string[] args)
        {
            StartClient();
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
            finally
            {
                client.Close();
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState as Socket;
            client.EndConnect(ar);

            var message = $"Hello from {client.LocalEndPoint}";

            client.BeginSend(Encoding.UTF8.GetBytes(message), 0, message.Length, SocketFlags.None, SendCallback, client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState as Socket;
            var count = client.EndSend(ar);

            Console.WriteLine("{0} bytes were sent to server", count);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}

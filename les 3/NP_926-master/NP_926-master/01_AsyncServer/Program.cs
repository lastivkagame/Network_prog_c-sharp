using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01_AsyncServer
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
        private static AutoResetEvent done = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            // Socket:
            // 1) new
            // 2) Bind, Connect
            // 3) Accept
            // 4) Receive
            // 5) Responce
            // 6) Shutdown
            StartServer();
        }
        private static void StartServer()
        {
            var server = new Socket(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var backlog = 20;

            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            try
            {
                server.Bind(new IPEndPoint(ip, port));
                server.Listen(backlog);

                while (true)
                {
                    Console.WriteLine("Wait for connection...");
                    server.BeginAccept(AcceptCallback, server);
                    done.WaitOne();
                }
            }
            catch (SocketException exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                server.Close();
            }

        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            done.Set();
            var server = ar.AsyncState as Socket;
            var client = server.EndAccept(ar);

            var data = new TransferObject
            {
                Socket = client,
                Buffer = new byte[TransferObject.size]
            };

            client.BeginReceive(data.Buffer, 0, TransferObject.size, SocketFlags.None, ReceiveCallback, data);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            var data = (TransferObject)ar.AsyncState;

            var count = data.Socket.EndReceive(ar);
            var message = Encoding.UTF8.GetString(data.Buffer, 0, count);

            Console.WriteLine("Server got: {1}, size {0} bytes", count, message);

            var responce = $"{DateTime.Now.ToShortDateString()}: got {count} bytes";

            data.Socket.BeginSend(Encoding.UTF8.GetBytes(responce), 0, responce.Length,
                                   SocketFlags.None, SendCallback, data.Socket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState as Socket;
            var count = client.EndSend(ar);

            Console.WriteLine("{0} bytes were sent to client", count);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}

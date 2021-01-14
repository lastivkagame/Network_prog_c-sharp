using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Async_soc_serv
{
    struct TransferObject
    {
        public Socket socket { get; set; }

        public byte[] Buffer { get; set; }

        public static readonly int size = 1024;
    }

    class Program
    {
        //private static Socket server;
        private static readonly int port = 2020;
        private static IPAddress ip;
        private static AutoResetEvent done = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            StartServer();
        }
        delegate int Calc(int a, int b);

        private static void StartServer()
        {
            var server = new Socket(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var backlog = 20;
            //Calc c = new Calc((a, b) => { return a + b; });
            //c(3, 5); // or you can c.Invoke(3,5);
            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            try
            {
                server.Bind(new IPEndPoint(ip, port));
                server.Listen(backlog);

                while (true)
                {
                    Console.WriteLine("Wait for connection ...");
                    server.BeginAccept(AcceptCallback, server);
                    done.WaitOne();
                }
            }
            catch (SocketException exeption)
            {
                Console.WriteLine(exeption.Message);
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
                socket = client,
                Buffer = new byte[TransferObject.size]
            };

            client.BeginReceive(data.Buffer, 0, TransferObject.size, SocketFlags.None, ReceiveCallback, data);
            //throw new NotImplementedException();
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            var data = (TransferObject)ar.AsyncState;
            //var count = client.EndAccept(ar);
            var count = data.socket.EndReceive(ar);
            var message = Encoding.UTF8.GetString(data.Buffer, 0, count);
            Console.WriteLine("Server got: {1}, {0} bytes", count, message);
            //throw new NotImplementedException();

            //var responce = DateTime.Now.ToShortDateString() + ": got " + count + "bytes";
            var responce =  "Some text: got " + count + "bytes";
            data.socket.BeginSend(Encoding.UTF8.GetBytes(responce), 0, responce.Length, SocketFlags.None, SendCallback, data.socket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState as Socket;
            var count = client.EndSend(ar);

            Console.WriteLine("{0} bytes were sent to client", count);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            //throw new NotImplementedException();
        }
    }
}

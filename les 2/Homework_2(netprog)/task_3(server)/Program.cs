using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace task_3_server_
{
    struct TransferObject
    {
        public Socket socket { get; set; }

        public byte[] Buffer { get; set; }

        public static readonly int size = 1024;
    }
    class Program
    {
        #region ForSocket

        private static readonly int port = 8080;
        private static IPAddress ip;
        private static AutoResetEvent done = new AutoResetEvent(false); 
        #endregion

        static void Main(string[] args)
        {
            Console.Title = "Server";
            StartServer();
        }

        private static void StartServer()
        {
            var server = new Socket(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var backlog = 20;
            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            try
            {
                server.Bind(new IPEndPoint(ip, port)); //зв'язуємось
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

        /// <summary>
        /// Починаємо приймати повідомлення надісланні клієнтом
        /// </summary>
        /// <param name="ar"></param>
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
        }

        /// <summary>
        /// Доприймаємо і виводимо повідомлення клієнта і починаємо надсилати відповідь
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            var data = (TransferObject)ar.AsyncState;
            var count = data.socket.EndReceive(ar);
            var message = Encoding.UTF8.GetString(data.Buffer, 0, count);
            Console.WriteLine("At " + DateTime.Now.ToShortTimeString() + " was receive stroke:  " + message);

            var responce = "HI) Have a nice day!";
            data.socket.BeginSend(Encoding.UTF8.GetBytes(responce), 0, responce.Length, SocketFlags.None, SendCallback, data.socket);
        }

        /// <summary>
        /// Донадсилаємо повідомлення клієнту і закриваємо з'єднання
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState as Socket;
            var count = client.EndSend(ar);

            Console.WriteLine("->Message was sent to client(lenth: {0} bytes)", count);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

    }
}

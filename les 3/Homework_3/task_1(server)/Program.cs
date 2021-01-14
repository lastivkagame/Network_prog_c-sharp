using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace task_1_server_
{
    struct TransferObject
    {
        public Socket socket { get; set; }

        public byte[] Buffer { get; set; }

        public static readonly int size = 1024;
    }

    class Program
    {
        private static readonly int port = 2020;
        private static IPAddress ip;
        private static AutoResetEvent done = new AutoResetEvent(false);
        public static StreetMailDB street_context = new StreetMailDB();
        static void Main(string[] args)
        {
            StartServer();
            Console.ReadLine();
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
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            var data = (TransferObject)ar.AsyncState;
            var count = data.socket.EndReceive(ar);
            var message = Encoding.UTF8.GetString(data.Buffer, 0, count);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("->server got: {1}, {0} bytes", count, message);
            Console.ForegroundColor = ConsoleColor.White;
            var responce = "";
            List<int> indexes = new List<int>();

            try
            {

                // отправляем ответ
                responce = ""; //= "Mess hello!";

                foreach (var item in street_context.DBMails)
                {
                    if (item.MailIndex == message.ToString())
                    {
                        indexes.Add(item.Id);
                        break;
                    }
                }

                foreach (var item in street_context.Streets)
                {
                    if (item.DBMailIndexID == indexes[0])
                    {
                        responce += item.Name + ", ";
                    }
                }

            }
            catch (Exception)
            {
                responce = "Nothing not found";
            }

            if (responce == "")
            {
                responce = "Sorry, there are no streets";
            }

            data.socket.BeginSend(Encoding.UTF8.GetBytes(responce), 0, responce.Length, SocketFlags.None, SendCallback, data.socket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState as Socket;
            var count = client.EndSend(ar);

            Console.WriteLine("-> {0} bytes were sent to client", count);
        }
    }
}

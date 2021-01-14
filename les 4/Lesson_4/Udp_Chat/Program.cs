using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udp_Chat
{
    class Program
    {
        static int myPort;
        static int remotePort;
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your port: ");
            myPort = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter remote port: ");
            remotePort = int.Parse(Console.ReadLine());

            var thread = new Thread(Receive);
            thread.IsBackground = true;
            thread.Start();
            Send();
        }

        private static void Send()
        {
            while (true)
            {
                UdpClient client = new UdpClient();
                Console.Write("Client: {0}: \t", myPort);
                var data = Console.ReadLine();
                client.Send(Encoding.UTF8.GetBytes(data), data.Length, "127.0.0.1", remotePort);
            }
        }

        private static void Receive()
        {
            IPEndPoint ep = null;
            UdpClient server = new UdpClient(myPort);

            while (true)
            {
                var data = server.Receive(ref ep);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}: \t {1} ", ep, Encoding.UTF8.GetString(data, 0, data.Length));
                Console.ForegroundColor = ConsoleColor.White;
            }
            //throw new NotImplementedException();
        }
    }
}

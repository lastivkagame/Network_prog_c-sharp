using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace task_1_client_
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient("127.0.0.1", 2020);
            Console.WriteLine("Enter text to send: ");
            var message = Console.ReadLine();
            client.Send(Encoding.UTF8.GetBytes(message), message.Length);

            Console.ReadLine();
        }
    }
}

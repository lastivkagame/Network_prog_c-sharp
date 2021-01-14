using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace task_1_server_
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient server = new UdpClient(2020);
            IPEndPoint rp = null;
            Console.WriteLine("Wait ...");
            var data = server.Receive(ref rp);
            Console.WriteLine("Got: "+ Encoding.UTF8.GetString(data, 0, data.Length));
        }
    }
}

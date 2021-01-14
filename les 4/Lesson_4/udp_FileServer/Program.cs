using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace udp_FileServer
{
    class Program
    {
        const int port = 2020;
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("Wait ...");
            var server = new UdpClient(port);
            IPEndPoint ep = null;
            var data = server.Receive(ref ep);
            var path = Encoding.UTF8.GetString(data);

            data = server.Receive(ref ep);

            File.WriteAllBytes(path, data);

            Console.ReadLine();
            server.Close();
        }
    }
}

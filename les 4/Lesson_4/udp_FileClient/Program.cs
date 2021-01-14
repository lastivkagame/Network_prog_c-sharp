using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace udp_FileClient
{
    class Program
    {
        const int port = 2020;
        static void Main(string[] args)
        {
            Console.Title = "Client";
            //var path = "winter2.jpg";
            var path = "winter.txt";

            //using (var webclient = new WebClient())
            //{
            //    var url = "https://cdn.images.express.co.uk/img/dynamic/130/590x/Winter-2019-1205590.jpg?r=1574001155679";
            //   webclient.DownloadFile(url, path);
            //}

            var client = new UdpClient();
            SendHeaders(path, client);
            
            Console.ReadLine();
            client.Close();
        }

        private static void SendHeaders(string path, UdpClient client)
        {
            var filename = Path.GetFileName(path);
            client.Send(Encoding.UTF8.GetBytes(filename), filename.Length, "127.0.0.1", port);
            SendBody(path, client);
            //throw new NotImplementedException();
        }

        private static void SendBody(string path, UdpClient client)
        {
            var data = File.ReadAllBytes(path);
            client.Send(data, data.Length, "127.0.0.1", port);
            //throw new NotImplementedException();
        }
    }
}

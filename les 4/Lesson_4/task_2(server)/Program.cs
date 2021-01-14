using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace task_2_server_
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
            Console.WriteLine("title: "+path);

            int count = (Convert.ToInt32(Encoding.UTF8.GetString(data)));
            Console.WriteLine("count: " + count);

            byte[] finaldata = new byte[count * 8000];
            byte[][] tempdata = new byte[count][];

            for (int i = 0; i < count; i++)
            {
                tempdata[i]= server.Receive(ref ep);
            }

            int k = 0;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < tempdata[i].Length; j++)
                {
                    finaldata[k++] = tempdata[i][j];
                }
                //tempdata[i]
            }

            File.WriteAllBytes(path, finaldata);
            

            Console.ReadLine();
            server.Close();
        }
    }
}

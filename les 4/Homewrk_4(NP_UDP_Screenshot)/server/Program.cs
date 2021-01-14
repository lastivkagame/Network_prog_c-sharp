using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        const int port = 2020;
        static void Main(string[] args)
        {
            Console.Title = "Server";


            StartServer();

        }

        public static void StartServer()
        {

            try
            {
                do
                {
                    Console.WriteLine("Wait ...");
                    using (var server = new UdpClient(port))
                    {
                        IPEndPoint ep = null;
                        var data = server.Receive(ref ep);
                        var path = Encoding.UTF8.GetString(data);

                        //отримуємо
                        data = server.Receive(ref ep);

                        // показуємо що в процесі зберігання
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("->Title: " + path);
                        Console.ForegroundColor = ConsoleColor.Gray;

                        // count - к-ть частин які пришли
                        int count = (Convert.ToInt32(Encoding.UTF8.GetString(data)));
                        Console.WriteLine("count: " + count);

                        //збираємо це все до купи
                        byte[] finaldata = new byte[count * 8000];
                        byte[][] tempdata = new byte[count][];

                        for (int i = 0; i < count; i++)
                        {
                            tempdata[i] = server.Receive(ref ep);
                        }

                        int k = 0;

                        for (int i = 0; i < count; i++)
                        {
                            for (int j = 0; j < tempdata[i].Length; j++)
                            {
                                finaldata[k++] = tempdata[i][j];
                            }
                        }

                        // зберігаємо
                        File.WriteAllBytes(path, finaldata);
                    }


                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        }

    }
}

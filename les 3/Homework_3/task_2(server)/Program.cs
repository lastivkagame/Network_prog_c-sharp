using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace task_2_server_
{
    class Program
    {
        private const int port = 2020;
        private static IPAddress ip;
        private static TcpListener server;
        static void Main(string[] args)
        {
            StartServer();
            Console.Title = "Server: " + server.Server.LocalEndPoint;

            Console.ReadLine();
        }

        private static void StartServer()
        {
            AppGameContext context = new AppGameContext();
            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            server = new TcpListener(ip, port);

            server.Start();

            string comand = "";
            bool tosendgendre = false;
            bool tosendgame = false;
            string clientvalue = "";

            while (true)
            {
                Console.WriteLine("Wait for connection....");
                try
                {
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected. ");

                    using (var stream = client.GetStream())
                    {
                        try
                        {
                            if (comand == "")
                            {
                                var serializer_comand = new XmlSerializer(typeof(string));
                                comand = Convert.ToString(serializer_comand.Deserialize(stream));

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(DateTime.Now.ToShortTimeString() + " we have comand:  " + comand);
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            else if (tosendgame)
                            {
                                tosendgame = false;

                                var serializer2 = new XmlSerializer(typeof(List<GameToUI>));
                                int idgendre = -1;

                                foreach (var item in context.Gendres)
                                {
                                    if (item.Name == clientvalue)
                                    {
                                        idgendre = item.Id;
                                        break;
                                    }
                                }

                                List<GameToUI> games = new List<GameToUI>();

                                foreach (var item in context.Games)
                                {
                                    if (item.GendreID == idgendre)
                                    {
                                        games.Add(new GameToUI { Id = item.Id, Name = item.Name, Owner = item.Owner, GendreID = item.GendreID });
                                    }
                                }

                                if (games.Count() > 0)
                                {
                                    serializer2.Serialize(stream, games);
                                }
                                else
                                {
                                    serializer2.Serialize(stream, null);
                                }

                                comand = "";
                            }
                            else if (tosendgendre)
                            {
                                tosendgendre = false;

                                var serializer = new XmlSerializer(typeof(string));
                                string resalt = "";
                                int idgendre = -1;

                                foreach (var item in context.Games)
                                {
                                    if (item.Name == clientvalue)
                                    {
                                        idgendre = item.GendreID;
                                        break;
                                    }
                                }

                                //context.Gendres.Find((x)=>x.)

                                foreach (var item in context.Gendres)
                                {
                                    if (item.Id == idgendre)
                                    {
                                        resalt = item.Name;
                                    }
                                }

                                if (resalt == "")
                                {
                                    resalt = "Nothing found!";
                                }

                                serializer.Serialize(stream, resalt);
                                comand = "";
                            }
                            else
                            {
                                if (comand.ToString() == "game")
                                {
                                    tosendgendre = true;

                                    var serializer = new XmlSerializer(typeof(string));
                                    clientvalue = serializer.Deserialize(stream).ToString();

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " we have data from UI: " + clientvalue);
                                    Console.ForegroundColor = ConsoleColor.Gray;

                                }

                                if (comand.ToString() == "gendre")
                                {
                                    tosendgame = true;

                                    var serializer = new XmlSerializer(typeof(string));
                                    clientvalue = serializer.Deserialize(stream).ToString();

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " we have data from UI: " + clientvalue);
                                    Console.ForegroundColor = ConsoleColor.Gray;

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

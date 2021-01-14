using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DBStreet
{
    class Program
    {
        static int port = 8080; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            StreetDB context;
            context = new StreetDB();

            IPAddress iPAddress = Dns.GetHostEntry("localhost").AddressList[1];//IPAddress.Parse("10.7.150.13");//Dns.GetHostEntry("localhost").AddressList[1];//IPAddress.Parse("127.0.0.1"); //localhost
            IPEndPoint ipPoint = new IPEndPoint(iPAddress, port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Server started! Waiting for connection...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();

                    // handler.Receive();
                    // handler.Send();

                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    List<int> indexes = new List<int>();

                    try
                    {

                        // отправляем ответ
                        string message = ""; //= "Mess hello!";

                        foreach (var item in context.DBMails)
                        {
                            if (item.MailIndex == builder.ToString())
                            {
                                indexes.Add(item.Id);
                                break;
                            }
                        }

                        foreach (var item in context.Streets)
                        {
                            if (item.DBMailIndexID == indexes[0])
                            {
                                message += item.Name + ", ";
                            }
                        }

                        data = Encoding.Unicode.GetBytes(message);
                        handler.Send(data);
                    }
                    catch (Exception)
                    {
                        data = Encoding.Unicode.GetBytes("Nothing not found");
                        handler.Send(data);
                    }

                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

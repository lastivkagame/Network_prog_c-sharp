using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace task_2_server_
{
    class Program
    {
        static int port = 8080; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            IPAddress iPAddress = Dns.GetHostEntry("localhost").AddressList[1];/*IPAddress.Parse("10.7.150.13");*///Dns.GetHostEntry("localhost").AddressList[1];//IPAddress.Parse("127.0.0.1"); //localhost
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

                    Console.WriteLine("At " + DateTime.Now.ToShortTimeString() + " from [" + iPAddress + "] was sended stroke: " + builder.ToString());

                    // отправляем ответ
                    string message = "";

                    if (builder.ToString() == "1")
                    {
                        message = DateTime.Now.ToShortDateString();
                    }
                    else
                    {
                        message = DateTime.Now.ToShortTimeString();
                    }
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);

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

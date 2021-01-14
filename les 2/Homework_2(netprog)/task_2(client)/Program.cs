using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace task_2_client_
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8080; // порт сервера
        static string address = Dns.GetHostEntry("localhost").AddressList[1].ToString();//"10.7.150.13"; // адрес сервера
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    // подключаемся к удаленному хосту
                    socket.Connect(ipPoint);
                    string message = "";

                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Please choose: ");
                        Console.WriteLine("1.Date");
                        Console.WriteLine("2.Time");
                        Console.WriteLine("3.Exit");
                        Console.Write("Answer: ");
                        message = Console.ReadLine();
                    } while (message != "1" && message != "2" && message != "3");

                    if (message != "3")
                    {
                        byte[] data = Encoding.Unicode.GetBytes(message);
                        socket.Send(data);

                        // получаем ответ
                        data = new byte[256]; // буфер для ответа
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0; // количество полученных байт

                        do
                        {
                            bytes = socket.Receive(data, data.Length, 0);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (socket.Available > 0);
                        Console.WriteLine("At " + DateTime.Now.ToShortTimeString() + " from [" + address + "] was sended stroke: " + builder.ToString());

                        // закрываем сокет
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Bue! Have a nice day)");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.Read();
        }
    }
}

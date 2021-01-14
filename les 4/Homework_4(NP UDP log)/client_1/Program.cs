using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace client_1
{
    class Program
    {
        //public static int port = 8080;
        public static string command = "I want text";
        //public static List<string> log = new List<string>();

        static string remoteAddress; // хост для отправки данных
        static int remotePort; // порт для отправки данных
        static int localPort; // локальный порт для прослушивания входящих подключений
        static string user_name = "";

        static void Main(string[] args)
        {
            Console.Title = "Client";

            try
            {
                Console.WriteLine("Порт для прослуховування: 8181"); // локальный порт
                localPort = 8181;//Int32.Parse(Console.ReadLine());
                Console.WriteLine("Адрес для пiдключення: 127.0.0.2");
                remoteAddress = "127.0.0.2";//Console.ReadLine(); // адрес, к которому мы подключаемся
                Console.WriteLine("Порт для подключення: 8080");
                remotePort = 8080;//Int32.Parse(Console.ReadLine()); // порт, к которому мы подключаемся

                Console.Write("Введiть iм'я: ");
                user_name = Console.ReadLine();

                if (user_name == "")
                {
                    user_name = "no name";
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Ви: ");

                SendName();

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                SendMessage(); // отправляем сообщение
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SendName()
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                byte[] data = Encoding.Unicode.GetBytes("#nm" + user_name);
                sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        private static void SendMessage()
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                while (true)
                {
                    string message = Console.ReadLine(); // сообщение для отправки
                    byte[] data = Encoding.Unicode.GetBytes(user_name + ": " + message);
                    sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        private static void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Вiдповiдь: {0}", message);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Ви: ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}

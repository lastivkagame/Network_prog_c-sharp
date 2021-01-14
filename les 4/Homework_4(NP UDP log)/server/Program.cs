using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace server
{
    class Program
    {
        public static List<string> quotes = new List<string>()
        {
            "Curabitur lobortis, lacus eget tempor sagittis, mi arcu ornare neque, non dapibus dolor ipsum a nibh.",
            "Cras ac ex vulputate sapien molestie iaculis ac eu ligula. Curabitur congue luctus tellus a porttitor.",
            "Cras convallis maximus ante tincidunt pellentesque. Ut ac euismod arcu.",
            "Nam sollicitudin faucibus lectus ac lacinia. Fusce eget sollicitudin tortor.",
            "Donec in enim bibendum, efficitur orci at, ornare turpis.",
            "Vestibulum nec ultricies elit, et efficitur orci.",
            "Curabitur viverra tempor bibendum.",
            "Maecenas pharetra ligula vel quam fringilla suscipit.",
            "Aenean facilisis augue sit amet turpis accumsan, ut mollis erat volutpat. Cras sit amet nunc nisl."
        };

        static string remoteAddress; // хост для отправки данных
        static int remotePort; // порт для отправки данных
        static int localPort; // локальный порт для прослушивания входящих подключений
        static List<string> logs_info = new List<string>();
        public static string messagefromuser = "";

        static void Main(string[] args)
        {
            Console.Title = "Server";

            try
            {
                List<string> log_info = new List<string>();

                Console.WriteLine("Port for listeting: 8080"); // локальный порт
                localPort = 8080;//Int32.Parse(Console.ReadLine());
                Console.WriteLine("Address for connection: 127.0.0.2");
                remoteAddress = "127.0.0.2";// Console.ReadLine(); // адрес, к которому мы подключаемся
                Console.WriteLine("Port fo connection: 8181");
                remotePort = 8181;//Int32.Parse(Console.ReadLine()); // порт, к которому мы подключаемся

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("I`m Wait ... ");

                log_info.Add("Port for listeting: 8080\n");
                log_info.Add("Address for connection: 127.0.0.2\n");
                log_info.Add("Port fo connection: 8181\n");
                log_info.Add("I`m Wait ... \n");

                SaveInfo(log_info);

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                SendMessage(); // отправляем сообщение
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Save log info in file
        /// </summary>
        /// <param name="arr"></param>
        private static void SaveInfo(List<string> arr)
        {
            string path = Directory.GetCurrentDirectory() + "//log.txt";

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (var item in arr)
                    {
                        sw.Write(item, 0, item.Length);
                    }
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    foreach (var item in arr)
                    {
                        sw.WriteLine(item, 0, item.Length);
                    }
                }
            }
        }

        
        private static void SendMessage()
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            Random rand = new Random();
            try
            {
                while (true)
                {
                    if (messagefromuser != "")
                    {
                        string message = quotes[rand.Next(0, quotes.Count)];//Console.ReadLine(); // сообщение для отправки
                        byte[] data = Encoding.Unicode.GetBytes(message);
                        sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
                        messagefromuser = "";
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Responce from server: " + message);
                        logs_info.Add("Responce from server: " + message);
                        SaveInfo(logs_info);
                        logs_info.Clear();
                    }
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


                    string name = "";
                    if (message[0] == '#' && message[1] == 'n' && message[2] == 'm')
                    {
                        for (int i = 3; i < message.Length; i++)
                        {
                            name += message[i];
                        }

                        message = name + " was add! (Time: " + DateTime.Now.ToShortTimeString() + " (" + DateTime.Now.ToShortDateString() + "))";
                    }
                    else
                    {
                        if (message != "")
                        {
                            messagefromuser = message;
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(message);
                    logs_info.Add(message);
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

        #region TryKnowTimeDisconectedClient
        //static class SocketExtensions
        //{
        //    public static bool IsConnected(Socket socket)
        //    {
        //        try
        //        {
        //            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        //        }
        //        catch (SocketException) { return false; }
        //    }
        //} 
        #endregion
    }
}
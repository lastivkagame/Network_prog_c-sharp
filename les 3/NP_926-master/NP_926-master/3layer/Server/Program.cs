using DAL;
using Server.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
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
        }

        private static void StartServer()
        {
            var dbHelper = new DbHelper();
            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            server = new TcpListener(ip, port);

            server.Start();

            while (true)
            {
                Console.WriteLine("Wait for connection....");
                try
                {
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected. ");

                    using (var stream = client.GetStream())
                    {
                        var serializer = new XmlSerializer(typeof(ContactDTO));
                        var contact = (ContactDTO)serializer.Deserialize(stream);

                        dbHelper.AddContact(new DAL.Entities.Contact
                        {
                            Email = contact.Email,
                            Name = contact.Name,
                            Phone = contact.Phone
                        });
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

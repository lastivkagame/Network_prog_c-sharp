using DAL;
using DAL.Entity;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/*Робота сервера: підключення - TcpListener.
Тоді чекає: отримує команду -> перепідключається надсилає/приймає в/з DAL */

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
            var dbHelper = new ContactHelper();
            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            server = new TcpListener(ip, port);

            server.Start();

            string comand = "";

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
                            else
                            {
                                if (comand.ToString() == "refresh")
                                {
                                    var serializer = new XmlSerializer(typeof(List<ContactByDB>));
                                    List<Contact> cont = dbHelper.GetContacts();

                                    List<ContactByDB> readycont = new List<ContactByDB>();

                                    foreach (var item in cont)
                                    {
                                        readycont.Add(new ContactByDB { Id = item.Id, Name = item.Name, Age = item.Age, City = item.City, SurName = item.SurName, Telephone = item.Telephone, ImageContact = item.ImageContact });
                                    }

                                    serializer.Serialize(stream, readycont);

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " we sended data to UI(->refresh)");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }

                                if (comand.ToString() == "add")
                                {
                                    var serializer = new XmlSerializer(typeof(ContactByDB));
                                    var item = (ContactByDB)serializer.Deserialize(stream);
                                    dbHelper.AddContact(new Contact { Id = item.Id, Name = item.Name, Age = item.Age, City = item.City, SurName = item.SurName, Telephone = item.Telephone, ImageContact = item.ImageContact });

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " we have data from UI(->add)");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }

                                if (comand.ToString() == "delete")
                                {
                                    var serializer = new XmlSerializer(typeof(ContactByDB));
                                    var item = (ContactByDB)serializer.Deserialize(stream);
                                    dbHelper.DelContact(new Contact { Id = item.Id, Name = item.Name, Age = item.Age, City = item.City, SurName = item.SurName, Telephone = item.Telephone, ImageContact = item.ImageContact });

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " we have data from UI(->delete)");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }

                                if (comand.ToString() == "edit")
                                {
                                    var serializer = new XmlSerializer(typeof(ContactByDB));
                                    var item = (ContactByDB)serializer.Deserialize(stream);

                                    dbHelper.UpdateContact(new Contact { Id = item.Id, Name = item.Name, Age = item.Age, City = item.City, SurName = item.SurName, Telephone = item.Telephone, ImageContact = item.ImageContact });

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " we have data from UI(->edit)");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }

                                comand = "";
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

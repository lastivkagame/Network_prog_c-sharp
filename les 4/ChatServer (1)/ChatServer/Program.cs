using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        
            static ServerObject server; // сервер
            static Thread listenThread; // потока для прослушивания
            static void Main(string[] args)
            {
                try
                {
                    server = new ServerObject();
                    listenThread = new Thread(new ThreadStart(server.Listen));
                    listenThread.Start(); 
                }
                catch (Exception ex)
                {
                    server.Disconnect();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    
}

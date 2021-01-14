using System;

namespace SimpleFTP_Server
{
    public class Program
    {
        public static void Main()
        {            
            var server = new Server();
            server.Listen();

            Console.ReadLine();
        }
    }
}

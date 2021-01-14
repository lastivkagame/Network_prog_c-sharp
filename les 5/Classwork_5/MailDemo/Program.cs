using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClientMailKit = MailKit.Net.Smtp.SmtpClient; //аліас

namespace MailDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //SMTP(old) - 587 port -> for email 
            //(POP3(old) - все забирає з сервера, iMAP -протокол вхідної пошти) - нема засобів для отримання пошти в дот неті
            //TestSmtp();
            //IMap();
            await MailKiteSmtp();
        }

        private async static Task MailKiteSmtp()
        {
            var smtp = new SmtpClientMailKit();
           // await smtp.ConnectAsync("smtp.gmail.com", 587, true);
            await smtp.ConnectAsync("smtp.gmail.com", 465, true);

            await smtp.AuthenticateAsync("projectsprog1@gmail.com", "qqwwee11!!");

            if (smtp.IsAuthenticated)
            {
                Console.WriteLine("OK");

            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dasha", "projectsprog1@gmail.com"));
            message.To.Add(new MailboxAddress("Bohdan", "vistrel460@gmail.com"));
            message.Subject = "How you doin?";
            message.Body = new TextPart("plain")
            {
                Text = @"Please work"
            };

            await smtp.SendAsync(message);
        }

        private static void IMap()
        {
            var imap = new ImapClient();
            imap.Connect("imap.gmail.com", 993, true);
            imap.Authenticate("projectsprog1@gmail.com", "qqwwee11!!");

            if (imap.IsAuthenticated)
            {
                Console.WriteLine("User OK");
            }

            var inbox = imap.Inbox;
            inbox.Open(MailKit.FolderAccess.ReadOnly);

            for (int i = inbox.Count - 1; i > inbox.Count - 10; i--)
            {
                var message = inbox.GetMessage(i);
                Console.WriteLine($"{message.From} - {message.Subject} - {message.Date}");
            }
        }

        private static void TestSmtp()
        {
            var client = new SmtpClient("smtp.gmail.com", 587);//587
            client.EnableSsl = true;
            //client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("projectsprog1@gmail.com", "qqwwee11!!");
            client.Port = 587;

            var message = new MailMessage();
            // message.To.Add(new MailAddress("prisyazhnyuk_v@itstep.academy"));
            message.To.Add(new MailAddress("shukira15@gmail.com"));
            message.To.Add("vistrel460@gmail.com");
            message.From = new MailAddress("projectsprog1@gmail.com");


            message.Subject = "test smtp client #5 from s";
            message.Body = DateTime.Now.ToShortDateString();
            var url = "randomrobot.png";
            message.Attachments.Add(new Attachment(url));

            try
            {
                client.Send(message);

                Console.WriteLine("Good!");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

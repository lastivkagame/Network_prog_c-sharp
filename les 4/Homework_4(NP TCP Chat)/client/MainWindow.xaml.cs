using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Messages messages = new Messages();
        public static bool IsISend = false;
        public static string sended_message = "";
        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

        public MainWindow()
        {
            InitializeComponent();

            //ініціалізація певних елемнтів ui
            messages.YouAsSender = "Name ...";
            messages.NewMessage = "Messsage ...";
            messages.AddSender(new Chat() { Person = "System: ", Message = " Chat wasn't created enter name and click connect" });

            btnSend.IsEnabled = false; //щоб не надсилав листи куди не треба
            this.DataContext = messages;
        }

        private void BtnConect_Click(object sender, RoutedEventArgs e)
        {
            #region SetName
            if (messages.YouAsSender == "" || messages.YouAsSender == "Name ...")
            {
                messages.YouAsSender = "Anonym";
                messages.AddSender(new Chat() { Person = "System: ", Message = "You enter inccorect name -> your name change on Anonym" });
            }

            messages.AddSender(new Chat() { Person = "System: ", Message = "You conected to chat" });
            userName = messages.YouAsSender;
            #endregion

            client = new TcpClient();

            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
                //Console.WriteLine("Добро пожаловать, {0}", userName);
                Thread receiveThread2 = new Thread(new ThreadStart(SendMessage));
                receiveThread2.Start();

                //щоб конектитись не міг бо він вже, але тепер може писати і надсилати
                btnConect.IsEnabled = false;
                btnSend.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                btnConect.IsEnabled = true;
                Disconnect();
            }
        }

        static void SendMessage()
        {
            while (true)
            {
                if (IsISend == true)
                {
                    string message = sended_message;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    IsISend = false;
                }
            }
        }

        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    #region Отримуємо
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    #endregion

                    #region Виводимо
                    App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    string name = "";
                                    string mess = "";
                                    bool flag = false;

                                    for (int i = 0; i < message.Length; i++)
                                    {
                                        if (flag == false)
                                        {
                                            name += message[i];

                                            if (message[i] == ':')
                                            {
                                                flag = true;
                                            }
                                        }
                                        else
                                        {
                                            mess += message[i];
                                        }

                                    }

                                    messages.AddSender(new Chat() { Person = name, Message = mess });//вывод сообщения
                                });
                    #endregion
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    MessageBox.Show("Подключение прервано!");
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (messages.NewMessage == "" || messages.NewMessage == " ")
            {
                messages.AddSender(new Chat { Person = "System: ", Message = "(now visible only for you) - > You cam`t send nonone symbol!" });
            }
            else
            {
                for (int i = 0; i < 3; i++) // просто треба(так працює а без цикла ні)((але часу зрозуміти чому у мене більше нема( sorry
                {
                    // флаг надсилати
                    IsISend = true;
                    sended_message = messages.NewMessage;

                    if (i == 1)
                    {
                        //вивід надісланого
                        messages.AddSender(new Chat { Person = messages.YouAsSender + ": ", Message = sended_message });
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
        }
    }
}

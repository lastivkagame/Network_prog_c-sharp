using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
using System.Xml.Serialization;

namespace task_2_client_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int port = 2020;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSearchGame_Click(object sender, RoutedEventArgs e)
        {
            var client = new TcpClient(Dns.GetHostName(), port);
            using (var stream = client.GetStream())
            {
                var serializer_comand = new XmlSerializer(typeof(string));
                serializer_comand.Serialize(stream, "game");
            }

            client = new TcpClient(Dns.GetHostName(), port);

            using (var stream = client.GetStream())
            {
                var serializer_comand = new XmlSerializer(typeof(string));
                serializer_comand.Serialize(stream, gametext.Text);
            }

            client = new TcpClient(Dns.GetHostName(), port);
            object message_gendre;

            using (var stream = client.GetStream())
            {
                var serializer_comand = new XmlSerializer(typeof(string));
                message_gendre = serializer_comand.Deserialize(stream);
            }

            MessageBox.Show(message_gendre.ToString(), "Find Gendre", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnSearchGendre_Click(object sender, RoutedEventArgs e)
        {
            var client = new TcpClient(Dns.GetHostName(), port);
            using (var stream = client.GetStream())
            {
                var serializer_comand = new XmlSerializer(typeof(string));
                serializer_comand.Serialize(stream, "gendre");
            }

            client = new TcpClient(Dns.GetHostName(), port);

            using (var stream = client.GetStream())
            {
                var serializer_comand = new XmlSerializer(typeof(string));
                serializer_comand.Serialize(stream, gendretext.Text);
            }

            client = new TcpClient(Dns.GetHostName(), port);
            List<GameToUI> games = new List<GameToUI>();
            string resalt = "";

            using (var stream = client.GetStream())
            {
                try
                {
                    var serializer_comand = new XmlSerializer(typeof(List<GameToUI>));
                    games = (List<GameToUI>)serializer_comand.Deserialize(stream);
                }
                catch (Exception)
                {
                    resalt = "Nothing found!";
                }
            }

            if (games == null)
            {
                resalt = "Nothing found!";
            }

            if (games.Count <= 0)
            {
                resalt = "Sorry, no game by it";
            }

            if (resalt == "")
            {
                foreach (var item in games)
                {
                    resalt += "-> " + item.Name + " (owner: " + item.Owner + "); \n";
                }
            }

            MessageBox.Show(resalt, "Resalt Game", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

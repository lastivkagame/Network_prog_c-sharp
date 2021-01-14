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

namespace Client
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var contact = new ContactDTO
            {
                Email = tbEmail.Text,
                Name = tbName.Text,
                Phone = tbPhone.Text
            };

            var client = new TcpClient(Dns.GetHostName(), port);
            using (var stream = client.GetStream())
            {
                var serializer = new XmlSerializer(contact.GetType());
                serializer.Serialize(stream, contact);
            }

            client.Close();
        }
    }
}

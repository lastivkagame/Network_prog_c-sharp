using Homework_6_wpf_.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using UI.Model;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContactBook viewModel = new ContactBook();
        BitmapImage default_image = null;
        private const int port = 2020;

        string telephone;
        int idselctedcontact = -1;
        bool foredit = false;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                default_image = ConvertByteArrayToBitmapImage(File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\ImagesSourse\\default_image"));
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, default image not found!");
            }

            Refresh();
        }

        /// <summary>
        /// If simple, see in Btnhowuse_Click
        /// (it read, check send to server Contact object)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (contactListBox.SelectedIndex == -1) return;

            if (contactListBox.SelectedIndex != viewModel.Contacts.Count() - 1)
            {
                MessageBox.Show("Please, click button HOW USE and read how add contact, thanks)))", "Instruction", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (viewModel.SelectedContact.Name == "" || viewModel.SelectedContact.Surname == "" || viewModel.SelectedContact.Telephone == "")
                {
                    MessageBox.Show("FullName and Telephone is important part! Please fill it!");
                }
                else
                {
                    Contact isexist;

                    int age = viewModel.SelectedContact.Age;
                    string city = viewModel.SelectedContact.City;
                    BitmapImage bitmapImage = viewModel.SelectedContact.ImageBitmap;
                    string telephone = viewModel.SelectedContact.Telephone;

                    viewModel.SelectedContact.Telephone = "";

                    try
                    {
                        isexist = viewModel.Contacts.FirstOrDefault(a => a.Telephone == telephone);
                    }
                    catch (Exception)
                    {
                        isexist = null;
                        viewModel.SelectedContact = new Contact() { Name = "Name", Age = 0, Surname = "LastName", City = "City", ImageBitmap = default_image, Telephone = "+1.. (telephone)" };
                    }

                    if (isexist != null)
                    {
                        MessageBox.Show("This phone alredy used!");
                        viewModel.SelectedContact = new Contact() { Name = "Name", Age = 0, Surname = "LastName", City = "City", ImageBitmap = default_image, Telephone = "+1.. (telephone)" };
                    }
                    else
                    {
                        viewModel.SelectedContact.Telephone = telephone;

                        if (age.ToString() == "")
                        {
                            age = 3;
                        }

                        if (city == "")
                        {
                            city = "none";
                        }

                        if (bitmapImage is null)
                        {
                            bitmapImage = default_image;
                        }

                        ContactByDB newcontact = new ContactByDB
                        {
                            Name = viewModel.SelectedContact.Name,
                            SurName = viewModel.SelectedContact.Surname,
                            Telephone = viewModel.SelectedContact.Telephone,

                            Age = age,
                            City = city,
                            ImageContact = ConvertBitmapToByteArray(bitmapImage),
                        };

                        var client = new TcpClient(Dns.GetHostName(), port);
                        using (var stream = client.GetStream())
                        {
                            var serializer_comand = new XmlSerializer(typeof(string));
                            serializer_comand.Serialize(stream, "add");
                        }

                        client = new TcpClient(Dns.GetHostName(), port);
                        using (var stream = client.GetStream())
                        {
                            var serializer = new XmlSerializer(typeof(ContactByDB));
                            serializer.Serialize(stream, newcontact);
                        }

                        viewModel.AddContact(new Contact() { Name = "Name", Age = 0, Surname = "LastName", City = "City", ImageBitmap = default_image, Telephone = "+1.. (telephone)" });
                        foredit = false;
                        Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// If simple, see in Btnhowuse_Click
        /// (it read, check send to server Contact object)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (contactListBox.SelectedIndex == -1) return;

            if (contactListBox.SelectedIndex == viewModel.Contacts.Count() - 1)
            {
                MessageBox.Show("Please, click button HOW USE and read how add contact, thanks)))", "Instruction", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {

                if (viewModel.SelectedContact.Telephone != telephone)
                {
                    MessageBox.Show("Sorry! You can't change telephone!");
                }
                else
                {
                    BitmapImage bitmapImage = viewModel.SelectedContact.ImageBitmap;

                    ContactByDB editcontact = new ContactByDB()
                    {
                        Name = viewModel.SelectedContact.Name,
                        SurName = viewModel.SelectedContact.Surname,
                        Age = viewModel.SelectedContact.Age,
                        City = viewModel.SelectedContact.City,
                        Telephone = viewModel.SelectedContact.Telephone,
                        ImageContact = ConvertBitmapToByteArray(bitmapImage)
                    };

                    var client = new TcpClient(Dns.GetHostName(), port);
                    using (var stream = client.GetStream())
                    {
                        var serializer_comand = new XmlSerializer(typeof(string));
                        serializer_comand.Serialize(stream, "edit");
                    }

                    client = new TcpClient(Dns.GetHostName(), port);
                    using (var stream = client.GetStream())
                    {
                        var serializer = new XmlSerializer(typeof(ContactByDB));

                        serializer.Serialize(stream, editcontact);
                    }

                    MessageBox.Show("Contact edit!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    foredit = false;
                }
            }

            Refresh();
        }

        /// <summary>
        /// If simple, see in Btnhowuse_Click
        /// (it read and del from list on UI, check send to server Contact object)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (contactListBox.SelectedIndex == viewModel.Contacts.Count() - 1)
            {
                MessageBox.Show("You can't delete it becouse it doesn't contact" +
                    "(it element use for create new contact(click HOW USE for more))", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (contactListBox.SelectedIndex == -1) return;

                ContactByDB removecontact = new ContactByDB()
                {
                    Name = viewModel.SelectedContact.Name,
                    SurName = viewModel.SelectedContact.Surname,
                    Age = viewModel.SelectedContact.Age,
                    City = viewModel.SelectedContact.City,
                    Telephone = viewModel.SelectedContact.Telephone,
                    ImageContact = viewModel.SelectedContact.ImageContact
                };

                var client = new TcpClient(Dns.GetHostName(), port);
                using (var stream = client.GetStream())
                {
                    var serializer_comand = new XmlSerializer(typeof(string));
                    serializer_comand.Serialize(stream, "delete");
                }

                client = new TcpClient(Dns.GetHostName(), port);
                using (var stream = client.GetStream())
                {
                    var serializer = new XmlSerializer(typeof(ContactByDB));

                    serializer.Serialize(stream, removecontact);
                }

                viewModel.RemoveContact(viewModel.SelectedContact);
            }

            foredit = false;
            Refresh();
        }

        private void BtnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                viewModel.SelectedContact.ImageBitmap = new BitmapImage(new Uri(op.FileName));
            }
        }

        #region Converters
        public static byte[] ConvertBitmapSourceToByteArray(string filepath)
        {
            var image = new BitmapImage(new Uri(filepath));
            byte[] data;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public static byte[] ConvertBitmapToByteArray(BitmapImage image)
        {
            byte[] data;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public static BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            try
            {
                var stream = new MemoryStream(bytes);
                stream.Seek(0, SeekOrigin.Begin);
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
            catch (Exception)
            {
                //sometimes picture of DB can be not correct and if it true thenfore user simple don't see it 
                //MessageBox.Show(ex.Message);
            }

            return null;
        } 
        #endregion

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            List<ContactByDB> list = new List<ContactByDB>();
            var client = new TcpClient(Dns.GetHostName(), port);
            using (var stream = client.GetStream())
            {
                var serializer_comand = new XmlSerializer(typeof(string));
                serializer_comand.Serialize(stream, "refresh");
            }

            client = new TcpClient(Dns.GetHostName(), port);
            using (var stream = client.GetStream())
            {
                var serializer = new XmlSerializer(typeof(List<ContactByDB>));
                list = (List<ContactByDB>)serializer.Deserialize(stream);
            }

            viewModel.ClearContacts();
            this.DataContext = null;
            for (int i = 0; i < list.Count; i++)
            {
                viewModel.AddContact(new Contact
                {
                    Name = list[i].Name,
                    Surname = list[i].SurName,
                    Age = list[i].Age,
                    Telephone = list[i].Telephone,
                    City = list[i].City,
                    ImageBitmap = ConvertByteArrayToBitmapImage(list[i].ImageContact)
                });
            }

            viewModel.AddContact(new Contact() { Name = "Name", Age = 0, Surname = "LastName", City = "City", ImageBitmap = default_image, Telephone = "+1.. (telephone)" });
            this.DataContext = viewModel;
        }

        /// <summary>
        /// Simple how use it program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnhowuse_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("1. If you want add: " +
                "choose last contact(where standart info) " +
                "-> change it values to your(in right panel) " +
                "-> click button ADD \n 2.If you want edit: choose contact -> " +
                "change values to your(in right panel) -> click button EDIT\n " +
                "3. If you want delete contact: choose cotact -> click button delete\n" +
                "Important: you can`t change telephone\n", "HOW USE IT", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Use if user change telephone  and not click on any button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                telephone = viewModel.SelectedContact.Telephone;
                idselctedcontact = viewModel.FindID(telephone);
            }
            catch (Exception) // it can be id I delete contact
            {
                telephone = "";
                idselctedcontact = 0;
                foredit = false;
            }

            if (foredit)
            {
                if (viewModel.CheckIfChange(telephone, idselctedcontact) == true)
                {
                    MessageBox.Show("You can't change telephone!");
                }
            }
            foredit = true;
        }
    }
}

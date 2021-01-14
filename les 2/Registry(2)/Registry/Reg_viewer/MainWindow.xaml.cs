using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Reg_viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RegistryKey[] regs = new[]
            {
                Registry.ClassesRoot,
                Registry.CurrentUser,
                Registry.LocalMachine,
                Registry.Users,
                Registry.CurrentConfig
            };

            foreach (var k in regs)
            {
                var item = new TreeViewItem()
                {
                    Header = k,
                    IsExpanded = false
                };
                tree.Items.Add(item);
                LoadSubKeys(item);

                item.Expanded += SubItem_Expanded;
            }
        }

        private void LoadSubKeys(TreeViewItem item)
        {
            RegistryKey key = (item.Header as RegistryKey);

            if (key == null) return;
            if (key.SubKeyCount <= 0) return;

            foreach (var name in key.GetSubKeyNames())
            {
                var subItem = new TreeViewItem()
                {                                        
                    IsExpanded = false,
                    //DisplayMemberPath = "Name"
                };

                try
                {
                    subItem.Header = key.OpenSubKey(name);
                }
                catch (Exception)
                { }

                subItem.Expanded += SubItem_Expanded;               

                item.Items.Add(subItem);
            }
        }

        private void SubItem_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (sender as TreeViewItem);
            foreach (TreeViewItem sub in item.Items)
            {
                LoadSubKeys(sub);
            }
        }
    }
}

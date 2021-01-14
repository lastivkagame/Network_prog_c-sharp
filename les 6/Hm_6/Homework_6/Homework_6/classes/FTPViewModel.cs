using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Homework_6.classes
{
    public class FTPViewModel
    {
        private readonly Command connect;
        private readonly Command create_directory;
        private readonly Command download_your_files;
        private readonly Command go_folder_up;
        private readonly Command go_folder_down;

        public ICommand GetConnect => connect;
        public ICommand CreateDirectory => create_directory;
        public ICommand DownloadYourFiles => download_your_files;
        public ICommand GoFolderUp => go_folder_up;
        public ICommand GoFolderDown => go_folder_down;

        private readonly Command rename_folder;
        private readonly Command delete_folder;
        private readonly Command download_folder;

        public ICommand RenameFolder => rename_folder;
        public ICommand DeleteFolder => delete_folder;
        public ICommand DownloadFolder => download_folder;

        public string DeleteImage { get; set; }
        public string DownloadImage { get; set; }

        /// <summary>
        /// Connect with URL and fill list
        /// </summary>
        /// <param name="url"></param>
        public static void ConnectToURL(string url)
        {
            try
            {
                folders_collection.Clear();
                //MessageBox.Show("Work connect");
                var request_connect = (FtpWebRequest)WebRequest.Create(url);

                request_connect.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                var responce = (FtpWebResponse)request_connect.GetResponse();
                var reader = new StreamReader(responce.GetResponseStream());
                var content = reader.ReadToEnd();
               // var content = "myfolder\nmy folder2\nfile3.txt\nsome folder\nfile.txt";
                string temp = "";
                bool flag = false;

                for (int i = 0; i < content.Length; i++)
                {
                    string image = "";

                    if (content[i] == '\n')
                    {
                        if (flag) //file
                        {
                            image = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\image\\file-icon.png";
                        }
                        else //folder
                        {
                            image = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\image\\canva-folder.png";
                        }

                        folders_collection.Add(new FolderElement { FolderName = temp, ImageBitmap = new BitmapImage(new Uri(image)) });

                        temp = "";
                        flag = false;
                    }

                    if (content[i] == '.')
                    {
                        flag = true;
                    }

                    temp += content[i];
                }

                MessageBox.Show("Status: \t" + responce.StatusDescription);
                reader.Close();
                responce.Close();
                //MessageBox.Show(PATH);
            }
            catch (Exception)
            {
            }
        }

        //public string NameFolderVisibility { get; set; }
        //public Visibility TextBoxFolderVisibility { get; set; }
        //public string TextBoxFolderName { get; set; }

        public FTPViewModel()
        {
            PATH = "ROOT:/";
            DeleteImage = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\image\\remove.png";
            DownloadImage = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\image\\download.png";

            connect = new DelegateCommand(() =>
            {
                ConnectToURL(HOST);
            });

            create_directory = new DelegateCommand(() =>
            {
                try
                {
                    //MessageBox.Show("Work");
                    var request = (FtpWebRequest)WebRequest.Create(HOST);
                    bool flag = false;
                    var folder = "My_Folder";

                    int k = 0;
                    do
                    {
                        flag = false;
                        string temp = folder + k++;

                        for (int i = 0; i < folders_collection.Count; i++)
                        {
                            if (folders_collection.ToList()[i].FolderName == temp)
                            {
                                flag = true;
                            }
                        }

                        if (flag == false)
                        {
                            folder = temp;
                        }

                    } while (flag);

                    var dialog = new DialogHelperWindow();
                    var folder_username = "";
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.ResponseText != "" && dialog.ResponseText != " " && dialog.ResponseText != "  ")
                        {
                            folder_username = dialog.ResponseText;
                        }
                    }

                    flag = true;
                    for (int i = 0; i < folders_collection.Count; i++)
                    {
                        if (folders_collection.ToList()[i].FolderName == folder_username)
                        {
                            flag = false;
                        }
                    }

                    if (flag)
                    {
                        if (folder_username != "")
                        {
                            folder = folder_username;
                        }
                    }

                    var image = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\image\\canva-folder.png";

                    for (int i = 0; i < folder.Length; i++)
                    {
                        if (folder[i] == '.')
                        {
                            image = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\image\\file-icon.png";
                        }
                    }

                    request = (FtpWebRequest)WebRequest.Create(Path.Combine(HOST, folder));
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;

                    var responce = (FtpWebResponse)request.GetResponse();
                    responce.Close();

                    folders_collection.Add(new FolderElement { FolderName = folder, ImageBitmap = new BitmapImage(new Uri(image)) });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            download_your_files = new DelegateCommand(() =>
            {
                try
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.FileName = "Document"; // Default file name
                    dlg.DefaultExt = ".txt"; // Default file extension
                    dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();
                    var file = ""; //"text_student.txt";
                                   // Process open file dialog box results
                    if (result == true)
                    {
                        file = dlg.FileName;
                    }

                    folders_collection.Add(new FolderElement
                    {
                        FolderName = Path.GetFileName(file),
                        ImageBitmap = new BitmapImage(new Uri(Directory.GetParent(Directory.GetParent
                        (Directory.GetCurrentDirectory()).FullName).FullName + "\\image\\file-icon.png"))
                    });

                    var request = (FtpWebRequest)WebRequest.Create(HOST);
                    request = (FtpWebRequest)WebRequest.Create(Path.Combine(HOST, file));
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    //MessageBox.Show("Work");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            go_folder_up = new DelegateCommand(() =>
            {
                //MessageBox.Show("Work");

                try
                {
                    if (PATH != "ROOT:/")
                    {
                        string resalt_folder = "";
                        int count_sl = 0;

                        for (int i = 0; i < PATH.Length; i++)
                        {
                            if (PATH[i] == '/')
                            {
                                count_sl++;
                            }
                        }

                        count_sl--;

                        for (int i = 0; i < PATH.Length; i++)
                        {
                            if (PATH[i] == '/')
                            {
                                count_sl--;
                            }

                            if (count_sl == 0)
                            {
                                break;
                            }

                            resalt_folder += PATH[i];
                        }

                        PATH = resalt_folder;

                        string foldertoconnect = "";
                        bool flag = false;

                        for (int i = 0; i < resalt_folder.Length; i++)
                        {
                            if (PATH[i] == '/')
                            {
                                flag = true;
                            }

                            if (flag)
                            {
                                foldertoconnect += PATH[i];
                            }
                        }

                        ConnectToURL(HOST + "/" + foldertoconnect);
                    }
                    else
                    {
                        MessageBox.Show("Sorry, you in root!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });

            go_folder_down = new DelegateCommand(() =>
            {
                try
                {
                    //MessageBox.Show("Work");

                    int index = -1;

                    //find element
                    for (int i = 0; i < folders_collection.Count; i++)
                    {
                        if (folders_collection.ToList()[i].FolderName == SelectedFolder.FolderName)
                        {
                            index = i;
                        }
                    }

                    PATH += folders_collection.ToList()[index].FolderName + @"/";

                    ConnectToURL(HOST + folders_collection.ToList()[index].FolderName + @"/");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            rename_folder = new DelegateCommand(() =>
            {
                //MessageBox.Show("Work");
                try
                {
                    var request = (FtpWebRequest)WebRequest.Create(HOST);
                    bool flag = false;
                    var folder = "My_Folder";

                    //if user enter uncorrect value set defalt
                    int k = 0;
                    do
                    {
                        flag = false;
                        string temp = folder + k++;

                        for (int i = 0; i < folders_collection.Count; i++)
                        {
                            if (folders_collection.ToList()[i].FolderName == temp)
                            {
                                flag = true;
                            }
                        }

                        if (flag == false)
                        {
                            folder = temp;
                        }

                    } while (flag);

                    var dialog = new DialogHelperWindow();
                    var folder_username = "";
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.ResponseText != "" && dialog.ResponseText != " " && dialog.ResponseText != "  ")
                        {
                            folder_username = dialog.ResponseText;
                        }
                    }

                    flag = true;
                    for (int i = 0; i < folders_collection.Count; i++)
                    {
                        if (folders_collection.ToList()[i].FolderName == folder_username)
                        {
                            flag = false;
                        }
                    }

                    if (flag)
                    {
                        if (folder_username != "")
                        {
                            folder = folder_username;
                        }
                    }

                    int index = -1;

                    for (int i = 0; i < folders_collection.Count; i++)
                    {
                        if (folders_collection.ToList()[i].FolderName == SelectedFolder.FolderName)
                        {
                            index = i;
                        }
                    }

                    folders_collection.ToList()[index].FolderName = folder;

                    request = (FtpWebRequest)WebRequest.Create(Path.Combine(HOST, folder));
                    request.Method = WebRequestMethods.Ftp.Rename;

                    request.RenameTo = folder;

                    var responce = (FtpWebResponse)request.GetResponse();
                    responce.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            delete_folder = new DelegateCommand(() =>
            {
                try
                {
                    //MessageBox.Show("Work");
                    var request = (FtpWebRequest)WebRequest.Create(HOST);

                    int index = -1;
                    //find element
                    for (int i = 0; i < folders_collection.Count; i++)
                    {
                        if (folders_collection.ToList()[i].FolderName == SelectedFolder.FolderName)
                        {
                            index = i;
                        }
                    }

                    folders_collection.Remove(folders_collection.ToList()[index]);

                    request = (FtpWebRequest)WebRequest.Create(Path.Combine(HOST, SelectedFolder.FolderName));
                    request.Method = WebRequestMethods.Ftp.DeleteFile;

                    var responce = (FtpWebResponse)request.GetResponse();
                    responce.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            download_folder = new DelegateCommand(() =>
            {
                //MessageBox.Show("Work");
                try
                {
                    var request = (FtpWebRequest)WebRequest.Create(HOST);

                    int index = -1;
                    //find element
                    for (int i = 0; i < folders_collection.Count; i++)
                    {
                        if (folders_collection.ToList()[i].FolderName == SelectedFolder.FolderName)
                        {
                            index = i;
                        }
                    }
                    MessageBox.Show("Folder " + SelectedFolder.FolderName + " was saved!");

                    request = (FtpWebRequest)WebRequest.Create(Path.Combine(HOST, SelectedFolder.FolderName));
                    request.Method = WebRequestMethods.Ftp.DownloadFile;

                    var responce = (FtpWebResponse)request.GetResponse();
                    responce.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            HOST = "ftp://92.52.138.128/";
            ConnectToURL(HOST);
        }

        static private readonly ICollection<FolderElement> folders_collection = new ObservableCollection<FolderElement>();

        public IEnumerable<FolderElement> FoldersCollections => folders_collection;

        public FolderElement SelectedFolder { get; set; }

        static public string HOST { get; set; }
        static public string PATH { get; set; }

    }
}

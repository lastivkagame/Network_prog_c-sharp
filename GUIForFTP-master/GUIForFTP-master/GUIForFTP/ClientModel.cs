namespace GUIForFTP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Класс, представляющий бизнес-логику клиента
    /// </summary>
    class ClientModel
    {
        /// <summary>
        /// Порт сервера
        /// </summary>
        private readonly string modelPort;

        /// <summary>
        /// Адрес сервера
        /// </summary>
        private readonly string modelAddress;

        /// <summary>
        /// Объект класса <see cref="ViewModel"/>
        /// </summary>
        private readonly ViewModel viewModel;

        /// <summary>
        /// Конструктор, задающий указанные пользователем порт, адрес сервера. Также задаёт объект VM
        /// </summary>
        /// <param name="portFromVM">Указанный пользователем порт сервера</param>
        /// <param name="addressFromVM">Указанный пользователем адресс сервера</param>
        /// <param name="viewModel">Объект ViewModel</param>
        public ClientModel(string portFromVM, string addressFromVM, ViewModel viewModel)
        {
            modelPort = portFromVM;
            modelAddress = addressFromVM;
            this.viewModel = viewModel;
        }         

        /// <summary>
        /// Стек для возврата на уровни выше
        /// </summary>
        private Stack<string> workingPath = new Stack<string>();              

        /// <summary>
        /// Директория, на которую "смотрит" сервер
        /// </summary>
        private string serverPath = "";

        /// <summary>
        /// Путь на данном шаге
        /// </summary>
        private string currentServerPath = "";

        /// <summary>
        /// Путь для сохранения файлов (объект из класса модели)
        /// </summary>
        public string pathToSaveFileModel = new DirectoryInfo(Directory.GetCurrentDirectory()).
                                    FullName + @"\GUIForFTPDonwload";

        /// <summary>
        /// Объект подключенного клиента
        /// </summary>
        private TcpClient GUIClient = new TcpClient();

        /// <summary>
        /// Поток для обмена информацией
        /// </summary>
        private NetworkStream GUIStream;

        /// <summary>
        /// Объект для чтения символов из потока
        /// </summary>
        private StreamReader GUIReader;

        /// <summary>
        /// Объект для записи символов в поток
        /// </summary>
        private StreamWriter GUIWriter;

        /// <summary>
        /// Подключение к новому серверу
        /// </summary>        
        public async Task ConnectToServerFirstTime()
        {
            if (GUIClient.Connected)
            {
                ShutdownGUIClient();
            }

            try
            {
                GUIClient = await Task.Run(() => new TcpClient(modelAddress, Convert.ToInt32(modelPort)));
                GUIStream = GUIClient.GetStream();
                GUIWriter = new StreamWriter(GUIStream);
                GUIReader = new StreamReader(GUIStream);
            }
            catch (SocketException)
            {
                throw new SocketException();
            }
        }

        /// <summary>
        /// Высвобождение ресурсов потока и отключение клиента
        /// </summary>
        private void ShutdownGUIClient()
        {
            GUIWriter.Close();
            GUIReader.Close();
            GUIStream.Close();
            GUIClient.Close();            
        }

        /// <summary>
        /// Получить путь, на который смотрит сервер
        /// </summary>
        public async Task GetServerPathOnConnectionToServer()
        {            
            Directory.CreateDirectory(pathToSaveFileModel);

            try
            {
                await GUIWriter.WriteLineAsync("path");
                await GUIWriter.WriteLineAsync("giveMePath");
                await GUIWriter.FlushAsync();

                serverPath = await GUIReader.ReadLineAsync();

                currentServerPath = serverPath;
            }
            catch (Exception)
            {
                ShutdownGUIClient();
                MessageBox.Show("Сервер оборвал соединение.");
            }                                                                         
        }

        /// <summary>
        /// Запрос серверу о получении коллекции файлов и папок 
        /// </summary>
        /// <param name="isUpdateTree">Этот вызов обновит существующее дерево?</param>
        /// <param name="addDirectoryToServerPath">Папка, которую выбрал пользователь</param>       
        public async Task ShowDirectoriesTree(bool isUpdateTree, string addDirectoryToServerPath)
        {                                    
            if (isUpdateTree)
            {                
                if (addDirectoryToServerPath == "..")
                {
                    currentServerPath = workingPath.Pop();                    
                }
                else
                {                    
                    workingPath.Push(currentServerPath);                    
                    currentServerPath += @"\" + addDirectoryToServerPath;
                }                
            }

            try
            {               
                await GUIWriter.WriteLineAsync("Listing");
                await GUIWriter.WriteLineAsync(currentServerPath);
                await GUIWriter.FlushAsync();
               
                var stringDirsAndFiles = await GUIReader.ReadLineAsync();

                var splitDirsAndFiles = stringDirsAndFiles.Split(' ');
                var dirStringWithSpace = splitDirsAndFiles[0].Replace("?", " ");
                var dirsArray = dirStringWithSpace.Split('/');
                var filesStringWithSpace = splitDirsAndFiles[1].Replace("?", " ");
                var filesArray = filesStringWithSpace.Split('/');

                viewModel.DirectoriesAndFiles.Clear();
                viewModel.isDirectory.Clear();

                if (currentServerPath != serverPath)
                {
                    viewModel.DirectoriesAndFiles.Add("..");
                    viewModel.isDirectory.Add(false);
                }
                foreach (string element in dirsArray)
                {
                    if (element != "")
                    {
                        viewModel.DirectoriesAndFiles.Add(element);
                        viewModel.isDirectory.Add(true);
                    }
                }
                foreach (string element in filesArray)
                {
                    if (element != "")
                    {
                        viewModel.DirectoriesAndFiles.Add(element);
                        viewModel.isDirectory.Add(false);
                    }
                }                
            }
            catch (Exception)
            {
                ShutdownGUIClient();
                MessageBox.Show("Сервер оборвал соединение.");
            }            
        }
        
        /// <summary>
        /// Запрос серверу на скачивание файла.
        /// </summary>
        /// <param name="fileName">Имя скачиваемого файла.</param> 
        /// <param name="isDownloadAll">Это вызов метода для загрузки всех файлов в папке?</param>
        public async Task DownloadFile(string fileName, bool isDownloadAll)
        {
            if (!isDownloadAll)
            {
                viewModel.Active.Add("Скачать файл");
            }

            TcpClient downloadClient = null;
            NetworkStream downloadStream = null;
            StreamWriter downloadWriter = null;
            StreamReader downloadReader = null;

            try
            {
                using (downloadClient = await Task.Run(() =>
                    new TcpClient(modelAddress, Convert.ToInt32(modelPort))))
                {
                    downloadStream = downloadClient.GetStream();
                    downloadWriter = new StreamWriter(downloadStream);
                    await downloadWriter.WriteLineAsync("Download");
                    await downloadWriter.WriteLineAsync(currentServerPath + @"\" + fileName);
                    await downloadWriter.FlushAsync();

                    viewModel.DownloadingFiles.Add(fileName);

                    downloadReader = new StreamReader(downloadStream);
                    string content = "";
                    try
                    {
                        content = await downloadReader.ReadToEndAsync();
                    }
                    catch
                    {                        
                        downloadReader.Close();
                        downloadWriter.Close();
                        downloadStream.Close();
                        downloadClient.Close();
                        viewModel.DownloadingFiles.Add(fileName + " не удалось скачать: сервер оборвал соединение.");
                        return;
                    }

                    using (var textFile = new StreamWriter(pathToSaveFileModel + @"\" + fileName))
                    {
                        textFile.WriteLine(content);
                    }

                    viewModel.DownloadedFiles.Add(fileName);
                }                  
            }
            catch (SocketException)
            {               
                viewModel.DownloadingFiles.Add($"Не удалось скачать файл {fileName}: подключение к серверу не удалось.");
            }
        }

        /// <summary>
        /// Запрос серверу о необходимости скачать все файлы в директории
        /// </summary>        
        public void DownloadAllFiles()
        {
            viewModel.Active.Add("Скачать все файлы");
            // в случае, если путь для загрузок выбран корректный, но до подключения к серверу.
            // И чтобы после этого и после подлючения к серверу можно было качать файлы в уже выбранный корректный путь 
            if (((MainWindow)Application.Current.MainWindow).textBoxSavePath.Text != "")
            {
                viewModel.PathToSaveFile = ((MainWindow)Application.Current.MainWindow).textBoxSavePath.Text;
            }

            var directoryInfo = new DirectoryInfo(currentServerPath);

            if (directoryInfo.GetFiles().Length > 0)
            {                
                foreach (FileInfo file in directoryInfo.GetFiles())
                {                   
                    new Task(async () => await DownloadFile(file.Name, true)).
                            Start(TaskScheduler.FromCurrentSynchronizationContext());                    
                }
            }            
        }
    }
}
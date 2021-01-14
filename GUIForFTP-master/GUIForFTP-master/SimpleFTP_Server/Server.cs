namespace SimpleFTP_Server
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// Класс, принимающий и обрабатывающий запросы о листинге и скачивании файлов
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Порт для подключения к серверу
        /// </summary>
        private const int port = 8888;

        /// <summary>
        /// IP адресс сервера
        /// </summary>
        private IPAddress localAdrress = IPAddress.Parse("127.0.0.1"); 

        /// <summary>
        /// Объект для мониторинга запросов
        /// </summary>
        private TcpListener listener;                 
        
        /// <summary>
        /// Перевод информации из массива строк в строку
        /// </summary>
        /// <param name="serializable">Массив информации</param>
        /// <returns>Строка с информацией</returns>
        private string Deserialize(string[] serializable)
        {
            string deserializeString = "";
            for (int i = 0; i < serializable.Length - 1; i++)
            {
                deserializeString += serializable[i] + " ";
            }
            deserializeString += serializable[serializable.Length - 1];

            return deserializeString;
        }

        /// <summary>
        /// Обработка запросов клиентов сервером
        /// </summary>
        public async void Listen()
        {           
            listener = new TcpListener(localAdrress, port);
            listener.Start();
            Console.WriteLine("Сервер слушает . . .");            
            while (true)
            {
                try
                {
                    var client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("Клиент подключился");

                    ThreadPool.QueueUserWorkItem(PerformRequest, client);                                                                                                                                                                                              
                }
                catch (Exception e)
                {                    
                    Console.WriteLine($"\n\nОшибка {e.Message}");
                    Console.WriteLine("\n\nСервер снова слушает . . .");
                }               
            }            
        }

        /// <summary>
        /// Выполнение запроса
        /// </summary>
        /// <param name="client">Объект клиента, который сделал запрос</param>
        private void PerformRequest(object client)
        {
            TcpClient currentClient = null;
            try
            {
                string request = string.Empty;
                do
                {
                    currentClient = (TcpClient)client;
                    var stream = currentClient.GetStream();
                    var reader = new StreamReader(stream);
                        request = reader.ReadLine();
                    var path = reader.ReadLine();

                    Console.WriteLine($"Получен запрос: вид - {request}, путь - {path}");
                    string answer;
                    var writer = new StreamWriter(stream);

                    switch (request)
                    {
                        case "Listing":
                            answer = Deserialize(GetArrayOfFilesAndDirectoies(path));                            
                            writer.WriteLine(answer);
                            writer.Flush();
                            Console.WriteLine($"Список файлов и папок отправлен.\n");                            
                            break;
                        case "Download":
                            answer = DownloadFile(path);                            
                            writer.WriteLine(answer);
                            writer.Flush();
                            Console.WriteLine($"Содержимое файла отправлено.\n");
                            currentClient.Close();
                            break;
                        case "path":
                            answer = new DirectoryInfo(Directory.GetCurrentDirectory()).
                                FullName;                            
                            writer.WriteLine(answer);
                            writer.Flush();
                            Console.WriteLine($"Путь, на который смотрит сервер, отправлен.\n");                            
                            break;
                    }
                }
                while (request != "Download");
            }
            catch (Exception e)
            {
                currentClient.Close();
            }
        }
        
        /// <summary>
        /// Получить массив файлов и папок
        /// </summary>
        /// <param name="path">Путь к директории</param>
        /// <returns>
        /// Двухэлементный массив, где        
        /// на первом месте - папки
        /// на втором месте - файлы.       
        /// Если директория не найдена, то вернётся одноэлементый массив с элементом "size=-1"
        /// </returns>
        private string[] GetArrayOfFilesAndDirectoies(string path)
        {
            DirectoryInfo directoryInfo;

            try
            {
                directoryInfo = new DirectoryInfo(path);
            }
            catch (DirectoryNotFoundException)
            {
                return new string[] { "size=-1" };
            }
            
            var answer = new string[2];

            if (directoryInfo.GetDirectories().Length > 0)
            {
                foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
                {
                    var directoryName = directory.Name.Replace(" ", "?");
                    answer[0] += directoryName + "/";                           
                }
                answer[0] = answer[0].TrimEnd('/');
            }
            else
            {
                answer[0] = "";
            }

            if (directoryInfo.GetFiles().Length > 0)
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    var fileName = file.Name.Replace(" ", "?");
                    answer[1] += fileName + "/";                               
                }
                answer[1] = answer[1].TrimEnd('/');
            }
            else
            {
                answer[1] = "";
            }

            return answer;
        }
        
        /// <summary>
        /// Ответ на запрос о содержимом файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Размер файла и его содержимое</returns>
        private string DownloadFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return "size=-1";
                }

                var answer = File.ReadAllText(path);

                return answer;
            }
            catch (Exception e )
            {
                return "Ошибка при скачивании файла: " + e.Message;
            }            
        }
    }
}
namespace GUIForFTP
{
    using System.Windows;

    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {        
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {            
            if (e.Exception.Message == "Путь имеет недопустимую форму.")
            {
                // Обработка кнопки "Скачать все файлы" после неудачного подключения 
                // (это подключение должно быть 2ым или более за время работы приложения)
                MessageBox.Show("Чтобы скачать все файлы в папке сначала необходимо подключится к серверу");
            }
            else
            {
                MessageBox.Show(e.Exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);                                
            }                                     
            e.Handled = true;
        }
    }
}

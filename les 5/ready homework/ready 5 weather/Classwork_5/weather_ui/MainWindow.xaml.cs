using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using weather_ui.Model;

namespace weather_ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WeatherView weatherView = new WeatherView();

        WeatherRoot weather_collection;

        public MainWindow()
        {
            InitializeComponent();

            var cityname = "London";

            SetNewCity(cityname);
            weatherView.City = cityname;
            FillCurrentWeather(cityname, weather_collection);
            FillList(cityname);

            this.DataContext = weatherView;
        }

        /// <summary>
        /// Підтягуємо погоду відповідно до міста
        /// </summary>
        /// <param name="cityname"></param>
        public void SetNewCity(string cityname)
        {
            #region ReadDataWeather
            var url_info = $"http://api.openweathermap.org/data/2.5/forecast?q={cityname}&units=metric&appid=744ef013a49f618d4320751b1af097a8";

            var request = WebRequest.CreateHttp(url_info);
            var responce = request.GetResponse();
            var stream = responce.GetResponseStream();
            var sr = new StreamReader(stream);
            var data = sr.ReadToEnd();

            weather_collection = JsonConvert.DeserializeObject<WeatherRoot>(data);
            #endregion
        }

        /// <summary>
        /// Заповнюємо поточну погоду 
        /// </summary>
        /// <param name="cityname"></param>
        /// <param name="user"></param>
        private void FillCurrentWeather(string cityname, WeatherRoot user)
        {
            #region SetCurrentWeatherInfo
            var today = DateTime.Today;

            float temp = user.list[0].main.temp;
            var newday = today.AddDays(0);
            var main_info = "Description: ";
            main_info += user.list[0].weather[0].main + ", ";
            main_info += user.list[0].weather[0].description + ". \n";
            main_info += "Windy: " + user.list[0].wind.speed + " m/s.\n";
            main_info += "Deg: " + user.list[0].wind.deg + ". \n";
            main_info += "Visibility: " + user.list[0].visibility + " k/m.\n";
            main_info += "Clouds: " + user.list[0].clouds.all + ". \n";
            main_info += "Humidity: " + user.list[0].main.humidity + "%. \n";
            main_info += "Pressure: " + user.list[0].main.pressure + "hPa. ";

            #region SetPictureToCurrentWeather

            //імя по якому будемо шукати
            var info = user.list.FirstOrDefault().weather.FirstOrDefault().icon;
            var picturename = info + ".png";

            //шукаємо
            var url_image = $"http://openweathermap.org/img/wn/{picturename}";//10d@4x.png

            //качаємо
            WebClient webClient = new WebClient();
            var data_picture = webClient.DownloadData(url_image);

            //встановлюємо
            //weatherView.currentweather.ImageIcon = ToImage(data_picture);
            #endregion
            weatherView.currentweather = null;

            weatherView.currentweather = new WeatherDay()
            {
                Temperature = temp,
                City = $"{cityname}({user.city.country})",
                Date = newday.ToShortDateString(),
                MainInfo = main_info,
                ImageIcon = ToImage(data_picture)
            };
            #endregion

            this.DataContext = weatherView;
        }

        /// <summary>
        /// Заповнюємо лист погоди
        /// </summary>
        /// <param name="city"></param>
        public void FillList(string city)
        {
            #region SetCurrentWeatherInfo
            var today = DateTime.Today;

            for (int i = 0; i < weather_collection.list.Length; i++)
            {
                float temp = weather_collection.list[i].main.temp;
                var newday = today.AddDays(i);
                var main_info = "Description: ";
                main_info += weather_collection.list[i].weather[0].main + ", ";
                main_info += weather_collection.list[i].weather[0].description + ". \n";
                main_info += "Windy: " + weather_collection.list[i].wind.speed + " m/s.\n";
                main_info += "Deg: " + weather_collection.list[i].wind.deg + ". \n";
                main_info += "Visibility: " + weather_collection.list[0].visibility + " k/m.\n";
                main_info += "Clouds: " + weather_collection.list[i].clouds.all + ". \n";
                main_info += "Humidity: " + weather_collection.list[i].main.humidity + "%. \n";
                main_info += "Pressure: " + weather_collection.list[i].main.pressure + "hPa. ";

                #region SetPictureToCurrentWeather

                //імя по якому будемо шукати
                var info = weather_collection.list[i].weather.FirstOrDefault().icon;
                var picturename = info + ".png";

                //шукаємо
                var url_image = $"http://openweathermap.org/img/wn/{picturename}";//10d@4x.png

                //качаємо
                WebClient webClient = new WebClient();
                var data_picture = webClient.DownloadData(url_image);

                #endregion

                weatherView.AddNextDayWeather(new WeatherDay()
                {
                    Temperature = temp,
                    City = $"{city}({weather_collection.city.country})",
                    Date = newday.ToShortDateString(),
                    MainInfo = main_info,
                    ImageIcon = ToImage(data_picture)
                });
                #endregion
            }
        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            weatherView.DeleteAll();

            SetNewCity(weatherView.City);
            //FillCurrentWeather(weatherView.City, weather_collection);
            FillList(weatherView.City);
        }
    }
}

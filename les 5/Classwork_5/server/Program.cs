using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using weather_ui;
using weather_ui.Model;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            UsingWebRequest();
        }

        private static void UsingWebRequest()
        {
            //var request = new HttpWebRequest()
            // var url = "https://randomuser.me/api/";

            var cityname = "London";
            var url_info = $"http://api.openweathermap.org/data/2.5/forecast?q={cityname}&units=metric&appid=744ef013a49f618d4320751b1af097a8";


            var request = WebRequest.CreateHttp(url_info);
            var responce = request.GetResponse();
            var stream = responce.GetResponseStream();
            var sr = new StreamReader(stream);
            var data = sr.ReadToEnd();

            var user = JsonConvert.DeserializeObject<WeatherRoot>(data);

            w = new WeatherView() { currentweather = new WeatherDay() { Temperature = user.list.FirstOrDefault().main.temp } };
            
            //Console.WriteLine($"City: { user.city.name }");
            //Console.WriteLine($"Clouds: { user.list.FirstOrDefault().clouds.all }");

            //Console.WriteLine($"Visibility: { user.list.FirstOrDefault().visibility }");
            //Console.WriteLine($"Visibility: { user.list.FirstOrDefault().weather.FirstOrDefault().description }");
            //Console.WriteLine($"Dt_txt: { user.list.FirstOrDefault().dt_txt }");
            //Console.WriteLine($"Temp: { user.list.FirstOrDefault().main.temp }");

            var info = user.list.FirstOrDefault().weather.FirstOrDefault().icon;

            //var picturename = "03d.png";
            var picturename = info + ".png";
            var url_image = $"http://openweathermap.org/img/wn/{picturename}";//10d@4x.png
            WebClient webClient = new WebClient();

            var data_picture = webClient.DownloadData(url_image);

            var temp = url_image.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var filename = temp[temp.Count() - 1];

            File.WriteAllBytes(filename, data_picture);
        }
        public static WeatherView w;

        public static WeatherView GetWeather()
        {
            return w;
        }
    }
}

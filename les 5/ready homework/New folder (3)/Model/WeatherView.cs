using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using weather_ui.Model;

namespace weather_ui
{
    public class WeatherView
    {
        private readonly ICollection<WeatherDay> wetherlist = new ObservableCollection<WeatherDay>();

        // Властивість для прив'язки до списку контактів
        public IEnumerable<WeatherDay> WeatherList => wetherlist;

        public WeatherDay currentweather { get; set; }

        public string City { get; set; }

        public void AddNextDayWeather(WeatherDay day)
        {
            wetherlist.Add(day);
        }

        public void DeleteAll()
        {
            wetherlist.Clear();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace weather_ui.Model
{
    public class WeatherDay : INotifyPropertyChanged
    {

        private float temperature;

        public float Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TemperatureForListBox));
            }
        }

        private string city;

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(InfoForListBox));
            }
        }

        private string shortMainInfo;

        public string MainInfo
        {
            get { return shortMainInfo;  }
            set
            {
                shortMainInfo = value;
                OnPropertyChanged();
            }
        }


        private string date;

        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(InfoForListBox));
            }
        }


        public string InfoForListBox => $"{City}, {Date}";
        public string TemperatureForListBox => $"{Temperature} °C";

        public byte[] beginimage;
        private BitmapImage image;

        public BitmapImage ImageIcon
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }

        // Оголошення події оновлення властивості
        public event PropertyChangedEventHandler PropertyChanged;

        // Створення методу OnPropertyChanged для виклику події
        // В якості параметра буде використано ім'я властивості, що його викликала.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

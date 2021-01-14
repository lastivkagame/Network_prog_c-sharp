using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public class Image : INotifyPropertyChanged
    {
        private string url;
        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private string link;

        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                OnPropertyChanged("Link");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Location : INotifyPropertyChanged
    {
        private string full;

        public string Full
        {
            get { return full; }
            set
            {
                full = value;
                OnPropertyChanged("Full");
            }
        }

        private string city;

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }

        private string state;

        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        private string country;

        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CurrentObservation : INotifyPropertyChanged
    {
        private Image image;

        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        private Location display_location;

        public Location Display_Location
        {
            get { return display_location; }
            set
            {
                display_location = value;
                OnPropertyChanged("Display_Location");
            }
        }

        private Location observation_location;

        public Location Observation_Location
        {
            get { return observation_location; }
            set
            {
                observation_location = value;
                OnPropertyChanged("Observation_Location");
            }
        }

        private string weather;

        public string Weather
        {
            get { return weather; }
            set
            {
                weather = value;
                OnPropertyChanged("Weather");
            }
        }

        private string temperature_string;

        public string Temperature_String
        {
            get { return temperature_string; }
            set
            {
                temperature_string = value;
                OnPropertyChanged("Temperature_String");
            }
        }

        private double temp_f;

        public double Temp_F
        {
            get { return temp_f; }
            set
            {
                temp_f = value;
                OnPropertyChanged("Temp_F");
            }
        }

        private double temp_c;

        public double Temp_C
        {
            get { return temp_c; }
            set
            {
                temp_c = value;
                OnPropertyChanged("Temp_C");
            }
        }

        private string wind_string;

        public string Wind_String
        {
            get { return wind_string; }
            set
            {
                wind_string = value;
                OnPropertyChanged("Wind_String");
            }
        }

        private string uv;

        public string UV
        {
            get { return uv; }
            set
            {
                uv = value;
                OnPropertyChanged("UV");
            }
        }


        private string icon;

        public string Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        private string icon_url;

        public string Icon_Url
        {
            get { return icon_url; }
            set
            {
                icon_url = value;
                OnPropertyChanged("Icon_Url");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class WeatherUnderground : INotifyPropertyChanged
    {
        private CurrentObservation current_observation;

        public CurrentObservation Current_Observation
        {
            get { return current_observation; }
            set
            {
                current_observation = value;
                OnPropertyChanged("Current_Observation");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WeatherUnderground()
        {
                Current_Observation = new CurrentObservation()
                {
                    Display_Location = new Location()
                    {
                        City = " ",
                        Country = " "
                    },
                    Observation_Location = new Location()
                    {
                        City = " ",
                        Country = " "
                    },
                    Temperature_String = " ",
                    UV = " ",
                    Weather = " ",
                };
            
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

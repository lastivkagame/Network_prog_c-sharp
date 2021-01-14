using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Homework_6_wpf_.Model
{
    public class Contact : INotifyPropertyChanged
    {
        private string name;
        private string surname;
        private int age;
        private string city;
        private string contactimage;
        private string telephone;
        private BitmapImage imagebitmap;

        public byte[] ImageContact { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                // якщо новий елемент рівний старому
                // тоді оновлення не відбувається, що уникає зайвого
                // оновлення компонентів інтерфейсу
                if (name != value)
                {
                    // оновлкння властивості
                    name = value;
                    // виклик події оновлення властивості Name
                    OnPropertyChanged();
                    // виклик події оновлення властивості InfoForListBox
                    OnPropertyChanged(nameof(InfoForListBox));
                }
            }
        }
        public string Surname
        {
            get { return surname; }
            set
            {
                if (surname != value)
                {
                    surname = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(InfoForListBox));
                }
            }
        }
        public int Age
        {
            get { return age; }
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged();
                }
            }
        }
        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(InfoForListBox));
                }
            }
        }

        public string ContactImage
        {
            get { return contactimage; }
            set
            {
                if (contactimage != value)
                {
                    contactimage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Telephone
        {
            get { return telephone; }
            set
            {
                if (telephone != value)
                {
                    telephone = value;
                    OnPropertyChanged();
                }
            }
        }

        public BitmapImage ImageBitmap
        {
            get { return imagebitmap; }
            set
            {
                if (imagebitmap != value)
                {
                    imagebitmap = value;
                    OnPropertyChanged();
                }
            }
        }

        // Оголошення події оновлення властивості
        public event PropertyChangedEventHandler PropertyChanged;

        // Створення методу OnPropertyChanged для виклику події
        // В якості параметра буде використано ім'я властивості, що його викликала.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string InfoForListBox => $"{Name} {Surname}";
        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }
}

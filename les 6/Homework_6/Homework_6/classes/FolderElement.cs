using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Homework_6.classes
{
    public class FolderElement : INotifyPropertyChanged
    {
        private string foldername;
        public string FolderName
        {
            get { return foldername; }
            set
            {
                if (foldername != value)
                {
                    foldername = value;
                    OnPropertyChanged();
                }
            }
        }

        private BitmapImage imagebitmap;
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
    }
}

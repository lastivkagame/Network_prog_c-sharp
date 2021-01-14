using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    //для ліста чату білдитись
    public class Chat : INotifyPropertyChanged
    {

        private string first_color;

        public string FirstColor
        {
            get { return first_color; }
            set { first_color = value; OnPropertyChanged(); }
        }


        private string this_person;

        public string Person
        {
            get { return this_person; }
            set { this_person = value; OnPropertyChanged(); }
        }

        private string message;

        public string  Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Messages
    {
        private readonly ICollection<Chat> senders = new ObservableCollection<Chat>();
        public IEnumerable<Chat> Senders => senders;

        private ICollection<string> people_enable = new ObservableCollection<string>();

        public IEnumerable<string> People_enable => people_enable;

        public string YouAsSender { get; set; }
        public string NewMessage { get; set; }
       
        public void AddSender(Chat c)
        {
            #region Logic foreground colors
            bool flag = true;

            foreach (var item in senders)
            {
                if (item.Person == c.Person)
                {
                    c.FirstColor = item.FirstColor;
                    flag = false;
                }
            }

            if (flag)
            {
                int temp2 = GetRandomIndexColor();
                int j = 0;

                do
                {
                    flag = false;
                    temp2 = GetRandomIndexColor();

                    for (int i = 0; i < used_color.Count; i++)
                    {
                        if (used_color[i] == temp2)
                        {
                            flag = true;
                        }
                    }

                    if (j == used_color.Count)
                    {
                        flag = false;
                        temp2 = GetRandomIndexColor();
                        used_color.Add(temp2);
                    }

                    j++;

                } while (flag);


                c.FirstColor = Colors[temp2];
                used_color.Add(temp2);
            } 
            #endregion

            senders.Add(c);

            #region Logic Add People and message
            string temp = "-> " + c.Person;
            temp = temp.Replace(":", "");
            flag = true;

            foreach (var item in people_enable)
            {
                if (item == temp)
                {
                    flag = false;
                }
            }

            if (flag)
            {
                people_enable.Add(temp);
            } 
            #endregion
        }
        public void DeleteSender(Chat c)
        {
            senders.Remove(c);
        }

        Random random = new Random();
        List<int> used_color = new List<int>();
        List<string> Colors = new List<string>()
        {
            "LightSeaGreen",
            "Orange",
            "Black",
            "DarkYellow",
            "Gold",
            "SteelBlue",
            "Pink"
        };

        public int GetRandomIndexColor()
        {
            return random.Next(0, Colors.Count() - 1);
        }
    }
}

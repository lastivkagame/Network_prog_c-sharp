using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_6_wpf_.Model
{
    public class ContactBook
    {
        // Колекція контактів має тип ObservableCollection,
        // який реалізує INotifyCollectionChanged і тим самим повідомляє 
        // про зміну колекції
        private ICollection<Contact> contacts = new ObservableCollection<Contact>();

        // Властивість для прив'язки до списку контактів
        public IEnumerable<Contact> Contacts => contacts;

        // Властивість для прив'язки до вибраного контакта
        public Contact SelectedContact { get; set; }
        public string SelectedColor { get; set; }
        public string FontFamilys { get; set; }

        // Метод додавання нового контакта в список
        public void AddContact(Contact c)
        {
            contacts.Add(c);
        }
        // Метод видалення контакта зі списоку
        public void RemoveContact(Contact c)
        {
            contacts.Remove(c);
        }

        public void ClearContacts()
        {
            contacts = new ObservableCollection<Contact>();
        }

        public int FindID(string telephone)
        {
            Contact[] find = contacts.ToArray();

            for (int i = 0; i < find.Length; i++)
            {
                if (find[i].Telephone == telephone)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool CheckIfChange(string telephone, int id)
        {
            Contact[] find = contacts.ToArray();

            for (int i = 0; i < find.Length; i++)
            {
                if (i == id)
                {
                    if (find[i].Telephone != telephone)
                    {
                        find[i].Telephone = telephone;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}

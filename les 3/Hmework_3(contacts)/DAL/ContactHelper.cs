using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ContactHelper
    {
        private readonly AppContext context;

        public ContactHelper()
        {
            context = new AppContext();
        }

        public void AddContact(Contact contact)
        {
            try
            {
                context.Contacts.Add(contact);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        public void DelContact(Contact contact)
        {
            try
            {
                var cont = context.Contacts.First(a => a.Telephone == contact.Telephone);
                context.Contacts.Remove(cont);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //Console.WriteLine(ex.Message);
            }
        }

        public void UpdateContact(Contact contact)
        {
            try
            {
                var cont = context.Contacts.First(a => a.Telephone == contact.Telephone);

                cont.Name = contact.Name;
                cont.SurName = contact.SurName;
                cont.Telephone = contact.Telephone;
                cont.ImageContact = contact.ImageContact;
                cont.Age = contact.Age;
                cont.City = contact.City;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();

            foreach (var item in context.Contacts)
            {
                contacts.Add(item);
            }
            return contacts;
        }
    }
}

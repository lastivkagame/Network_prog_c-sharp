using DAL.Entities;

namespace DAL
{
    public class DbHelper
    {
        private readonly ApplicationContext context;

        public DbHelper()
        {
            context = new ApplicationContext();
        }

        // CRUD - to do
        public void AddContact(Contact contact)
        {
            context.Contacts.Add(contact);
            context.SaveChanges();
        }

    }
}

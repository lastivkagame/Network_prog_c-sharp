namespace DAL
{
    using DAL.Entities;
    using System.Data.Entity;

    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
            : base("name=ApplicationContext")
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
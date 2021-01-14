namespace DAL
{
    using DAL.Entity;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class AppContext : DbContext
    {
        public AppContext()
            : base("DBContacts")
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; }
    }

}
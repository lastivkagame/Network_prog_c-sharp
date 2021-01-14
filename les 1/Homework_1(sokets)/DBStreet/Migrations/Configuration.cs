namespace DBStreet.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DBStreet.StreetDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DBStreet.StreetDB context)
        {
            //  This method will be called after migrating to the latest version.
            context.DBMails.Add(new DBMailIndex
            {
                Id = 1,
                MailIndex = "33451"
            });
            context.DBMails.Add(new DBMailIndex
            {
                Id = 2,
                MailIndex = "55432"
            });
            context.DBMails.Add(new DBMailIndex
            {
                Id = 3,
                MailIndex = "976754"
            });

            //context.SaveChanges();

            context.Streets.AddOrUpdate(new Street
            {
                Id = 1,
                Name = "Sv.Willsons Str.",
                DBMailIndexID = 2
            });
            context.Streets.AddOrUpdate(new Street
            {
                Id = 2,
                Name = "Cafedral Str.",
                DBMailIndexID = 1
            });
            context.Streets.AddOrUpdate(new Street
            {
                Id = 3,
                Name = "Sv. Elena Str.",
                DBMailIndexID = 3
            });

            context.Streets.AddOrUpdate(new Street
            {
                Id = 4,
                Name = "Sant Lui Str.",
                DBMailIndexID = 3
            });

            context.Streets.AddOrUpdate(new Street
            {
                Id = 5,
                Name = "Wedton Str.",
                DBMailIndexID = 1
            });
            //context.SaveChanges();
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}

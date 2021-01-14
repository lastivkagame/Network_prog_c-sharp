namespace DBStreet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class StreetDB : DbContext
    {
        // Your context has been configured to use a 'StreetDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DBStreet.StreetDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'StreetDB' 
        // connection string in the application configuration file.
        public StreetDB()
            : base("StreetDB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Street> Streets { get; set; }
        public virtual DbSet<DBMailIndex> DBMails { get; set; }
    }

    [Table("tblDBMailIndex")]
    public class DBMailIndex
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string MailIndex { get; set; }
        //public string  { get; set; }
        public virtual ICollection<Street> Streets { get; set; }
    }

    [Table("tblStreet")]
    public class Street
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }

        [ForeignKey("DBMailIndex")]
        public int DBMailIndexID { get; set; }

        public virtual DBMailIndex DBMailIndex { get; set; }
    }
}
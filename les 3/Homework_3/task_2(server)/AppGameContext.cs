namespace task_2_server_
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class AppGameContext : DbContext
    {
        // Your context has been configured to use a 'AppGameContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'task_2_server_.AppGameContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AppGameContext' 
        // connection string in the application configuration file.
        public AppGameContext()
            : base("AppGameContext")
        {
            //Database.SetInitializer(new GameDBInitializer());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Gendre> Gendres { get; set; }
        public virtual DbSet<Game> Games { get; set; }
    }

    //public class GameDBInitializer : DropCreateDatabaseAlways<AppGameContext>
    //{
    //    protected override void Seed(AppGameContext context)
    //    {
    //        IList<Gendre> defaultStandards = new List<Gendre>();

    //        defaultStandards.Add(new Gendre() { Id = 1, Name = "Action", Description = " Very popular #1(28 % at USA)" });
    //        defaultStandards.Add(new Gendre() { Id = 2, Name = "Shooter", Description = " Very popular #2 (21 % at USA)" });
    //        defaultStandards.Add(new Gendre() { Id = 3, Name = "Sport", Description = " Very popular #3(13 % at USA)" });

    //        context.Gendres.AddRange(defaultStandards);

    //        IList<Game> defaultStandards2 = new List<Game>();

    //        defaultStandards2.Add(new Game() { Id = 1, Name = "Game #1", Owner = "Mr.First", GendreID = 1 });
    //        defaultStandards2.Add(new Game() { Id = 2, Name = "Game #2", Owner = "Mr.Second", GendreID = 2 });
    //        defaultStandards2.Add(new Game() { Id = 3, Name = "Game #3", Owner = "Mr.Third", GendreID = 1 });

    //        context.Games.AddRange(defaultStandards2);

    //        base.Seed(context);
    //    }
    //}

    //[Serializable]
    [Table("tblGendre")]
    public class Gendre
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }

    //[Serializable]
    [Table("tblGame")]
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Owner { get; set; }

        [ForeignKey("Gendre")]
        public int GendreID { get; set; }

        public virtual Gendre Gendre { get; set; }
    }
}
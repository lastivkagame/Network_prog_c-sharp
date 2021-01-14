namespace task_1_server_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblDBMailIndex",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MailIndex = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblStreet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        DBMailIndexID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDBMailIndex", t => t.DBMailIndexID, cascadeDelete: true)
                .Index(t => t.DBMailIndexID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblStreet", "DBMailIndexID", "dbo.tblDBMailIndex");
            DropIndex("dbo.tblStreet", new[] { "DBMailIndexID" });
            DropTable("dbo.tblStreet");
            DropTable("dbo.tblDBMailIndex");
        }
    }
}

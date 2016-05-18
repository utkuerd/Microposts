namespace Microposts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MicropostModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Microposts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => new { t.Id, t.CreatedAt }, name: "IdAndCreatedAt")
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Microposts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Microposts", new[] { "User_Id" });
            DropIndex("dbo.Microposts", "IdAndCreatedAt");
            DropTable("dbo.Microposts");
        }
    }
}

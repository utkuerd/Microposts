namespace Microposts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MicropostValidations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Microposts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Microposts", new[] { "User_Id" });
            AlterColumn("dbo.Microposts", "Content", c => c.String(maxLength: 140));
            AlterColumn("dbo.Microposts", "User_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Microposts", "User_Id");
            AddForeignKey("dbo.Microposts", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Microposts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Microposts", new[] { "User_Id" });
            AlterColumn("dbo.Microposts", "User_Id", c => c.Int());
            AlterColumn("dbo.Microposts", "Content", c => c.String());
            CreateIndex("dbo.Microposts", "User_Id");
            AddForeignKey("dbo.Microposts", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}

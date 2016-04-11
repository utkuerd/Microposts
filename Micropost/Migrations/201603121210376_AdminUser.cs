namespace Micropost.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AdminUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Admin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Admin");
        }
    }
}

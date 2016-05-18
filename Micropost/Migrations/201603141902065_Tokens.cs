namespace Microposts.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Tokens : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "RememberToken");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "RememberToken", c => c.String());
        }
    }
}

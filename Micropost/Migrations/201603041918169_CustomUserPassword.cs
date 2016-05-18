namespace Microposts.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class CustomUserPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordDigest", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PasswordDigest");
        }
    }
}

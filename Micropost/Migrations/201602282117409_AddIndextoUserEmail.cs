namespace Microposts.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddIndextoUserEmail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 254));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "Name", c => c.String());
        }
    }
}

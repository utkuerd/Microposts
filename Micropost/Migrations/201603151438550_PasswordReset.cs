namespace Micropost.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class PasswordReset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ResetDigest", c => c.String());
            AddColumn("dbo.Users", "ResetSentAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ResetSentAt");
            DropColumn("dbo.Users", "ResetDigest");
        }
    }
}

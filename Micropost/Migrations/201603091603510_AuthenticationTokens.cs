namespace Micropost.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthenticationTokens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RememberToken", c => c.String());
            AddColumn("dbo.Users", "RememberDigest", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RememberDigest");
            DropColumn("dbo.Users", "RememberToken");
        }
    }
}

namespace Micropost.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountActivation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ActivationDigest", c => c.String());
            AddColumn("dbo.Users", "Activated", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ActivatedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ActivatedAt");
            DropColumn("dbo.Users", "Activated");
            DropColumn("dbo.Users", "ActivationDigest");
        }
    }
}

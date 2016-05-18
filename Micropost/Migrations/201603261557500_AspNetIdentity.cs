namespace Microposts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspNetIdentity : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String(nullable: false, maxLength: 50));
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 254),
                        PasswordDigest = c.String(),
                        RememberDigest = c.String(),
                        Admin = c.Boolean(nullable: false),
                        ActivationDigest = c.String(),
                        Activated = c.Boolean(nullable: false),
                        ActivatedAt = c.DateTime(),
                        ResetDigest = c.String(),
                        ResetSentAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String());
            CreateIndex("dbo.Users", "Email", unique: true);
        }
    }
}

namespace Microposts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FollowingUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Relationships",
                c => new
                    {
                        FollowerId = c.Int(nullable: false),
                        FollowedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FollowerId, t.FollowedId })
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowedId)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowedId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Relationships", "FollowedId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Relationships", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Relationships", new[] { "FollowedId" });
            DropIndex("dbo.Relationships", new[] { "FollowerId" });
            DropTable("dbo.Relationships");
        }
    }
}

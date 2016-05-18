namespace Microposts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MicropostContentRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Microposts", "Content", c => c.String(nullable: false, maxLength: 140));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Microposts", "Content", c => c.String(maxLength: 140));
        }
    }
}

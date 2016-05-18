namespace Microposts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageURL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Microposts", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Microposts", "Image");
        }
    }
}

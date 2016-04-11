namespace Micropost.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UniqueIndexForUserEmail : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
        }
    }
}

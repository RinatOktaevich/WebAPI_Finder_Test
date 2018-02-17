namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountLikes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Audios", "CountLikes", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Audios", "CountLikes");
        }
    }
}

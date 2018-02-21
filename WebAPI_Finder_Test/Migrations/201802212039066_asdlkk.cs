namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdlkk : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Audios", "CountLikes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Audios", "CountLikes", c => c.Int());
        }
    }
}

namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageUrlproptoCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ImageUrl");
        }
    }
}

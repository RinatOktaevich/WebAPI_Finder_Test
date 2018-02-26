namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Collapseaudioandcategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Audios", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Audios", "CategoryId");
            AddForeignKey("dbo.Audios", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Audios", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Audios", new[] { "CategoryId" });
            DropColumn("dbo.Audios", "CategoryId");
        }
    }
}

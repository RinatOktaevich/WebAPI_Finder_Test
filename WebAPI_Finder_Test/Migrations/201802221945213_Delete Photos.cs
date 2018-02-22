namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletePhotos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Photos", new[] { "ApplicationUserId" });
            DropTable("dbo.Photos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Photos", "ApplicationUserId");
            AddForeignKey("dbo.Photos", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}

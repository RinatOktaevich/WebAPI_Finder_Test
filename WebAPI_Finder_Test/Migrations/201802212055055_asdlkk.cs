namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdlkk : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Photos", new[] { "ApplicationUserId" });
            DropColumn("dbo.AspNetUsers", "Login");
            DropColumn("dbo.AspNetUsers", "AvatarImage");
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
            
            AddColumn("dbo.AspNetUsers", "AvatarImage", c => c.String());
            AddColumn("dbo.AspNetUsers", "Login", c => c.String());
            CreateIndex("dbo.Photos", "ApplicationUserId");
            AddForeignKey("dbo.Photos", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}

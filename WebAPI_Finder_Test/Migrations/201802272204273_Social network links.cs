namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Socialnetworklinks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SocialNetworks",
                c => new
                    {
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        Instagram = c.String(),
                        Vk = c.String(),
                        FaceBook = c.String(),
                        YouTube = c.String(),
                        Twitter = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId,cascadeDelete:true)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialNetworks", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.SocialNetworks", new[] { "ApplicationUserId" });
            DropTable("dbo.SocialNetworks");
        }
    }
}

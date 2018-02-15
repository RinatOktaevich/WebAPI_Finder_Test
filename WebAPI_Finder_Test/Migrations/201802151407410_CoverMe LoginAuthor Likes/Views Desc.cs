namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoverMeLoginAuthorLikesViewsDesc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AudioId = c.Int(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audios", t => t.AudioId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.AudioId)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Audios", "Description", c => c.String(maxLength: 100));
            AddColumn("dbo.Audios", "AuthorLogin", c => c.String(nullable: false));
            AddColumn("dbo.Audios", "ImageCover", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "AudioId", "dbo.Audios");
            DropIndex("dbo.Likes", new[] { "User_Id" });
            DropIndex("dbo.Likes", new[] { "AudioId" });
            DropColumn("dbo.Audios", "ImageCover");
            DropColumn("dbo.Audios", "AuthorLogin");
            DropColumn("dbo.Audios", "Description");
            DropTable("dbo.Likes");
        }
    }
}

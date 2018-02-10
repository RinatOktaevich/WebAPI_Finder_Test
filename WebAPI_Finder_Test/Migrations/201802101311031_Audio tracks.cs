namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Audiotracks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 128),
                        Performer = c.String(nullable: false, maxLength: 50),
                        Title = c.String(nullable: false, maxLength: 50),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Audios", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Audios", new[] { "User_Id" });
            DropTable("dbo.Audios");
        }
    }
}

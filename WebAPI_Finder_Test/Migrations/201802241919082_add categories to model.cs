namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addcategoriestomodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);

            CreateTable(
                "dbo.CategoryApplicationUsers",
                c => new
                {
                    Category_Id = c.Int(nullable: false),
                    ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.Category_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Category_Id, unique: false, clustered: false)
                .Index(t => t.ApplicationUser_Id, unique: false, clustered: false);

        }

        public override void Down()
        {
            DropForeignKey("dbo.CategoryApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CategoryApplicationUsers", "Category_Id", "dbo.Categories");
            DropIndex("dbo.CategoryApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CategoryApplicationUsers", new[] { "Category_Id" });
            DropIndex("dbo.Categories", new[] { "Name" });
            DropTable("dbo.CategoryApplicationUsers");
            DropTable("dbo.Categories");
        }
    }
}

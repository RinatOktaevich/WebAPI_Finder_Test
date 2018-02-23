namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryCityRegistrationdatetouser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "CityId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "RegistrationDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.AspNetUsers", "CityId");
            AddForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
            DropColumn("dbo.AspNetUsers", "RegistrationDate");
            DropColumn("dbo.AspNetUsers", "CityId");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
        }
    }
}

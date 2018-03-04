namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndexonLogin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Login", c => c.String(maxLength: 300));
            CreateIndex("dbo.AspNetUsers", "Login");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", new[] { "Login" });
            AlterColumn("dbo.AspNetUsers", "Login", c => c.String());
        }
    }
}

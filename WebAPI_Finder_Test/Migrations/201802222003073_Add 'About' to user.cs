namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAbouttouser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "About", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "About");
        }
    }
}

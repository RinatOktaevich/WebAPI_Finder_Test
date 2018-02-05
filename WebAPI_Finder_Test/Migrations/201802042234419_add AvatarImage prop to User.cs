namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAvatarImageproptoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AvatarImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AvatarImage");
        }
    }
}

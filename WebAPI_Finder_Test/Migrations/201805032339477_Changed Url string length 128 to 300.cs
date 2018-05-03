namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUrlstringlength128to300 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Audios", "Url", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Audios", "Url", c => c.String(nullable: false, maxLength: 128));
        }
    }
}

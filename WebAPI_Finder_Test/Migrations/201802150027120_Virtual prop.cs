namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Virtualprop : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Audios", name: "User_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Audios", name: "IX_User_Id", newName: "IX_ApplicationUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Audios", name: "IX_ApplicationUserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Audios", name: "ApplicationUserId", newName: "User_Id");
        }
    }
}

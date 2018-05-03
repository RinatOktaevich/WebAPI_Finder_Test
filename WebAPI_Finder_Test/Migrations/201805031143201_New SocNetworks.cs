namespace WebAPI_Finder_Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewSocNetworks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SocialNetworks", "iTunes", c => c.String());
            AddColumn("dbo.SocialNetworks", "SoundCloud", c => c.String());
            DropColumn("dbo.SocialNetworks", "Vk");
            DropColumn("dbo.SocialNetworks", "Twitter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SocialNetworks", "Twitter", c => c.String());
            AddColumn("dbo.SocialNetworks", "Vk", c => c.String());
            DropColumn("dbo.SocialNetworks", "SoundCloud");
            DropColumn("dbo.SocialNetworks", "iTunes");
        }
    }
}

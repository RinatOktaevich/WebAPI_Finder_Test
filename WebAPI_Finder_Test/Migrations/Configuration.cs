namespace WebAPI_Finder_Test.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebAPI_Finder_Test.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WebAPI_Finder_Test.Models.ApplicationDbContext context)
        {
            foreach (var item in context.Users)
            {
                item.FullName = item.FullName.ToLower();
            }


        }
    }
}

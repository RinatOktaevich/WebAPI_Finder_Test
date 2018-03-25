using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Diagnostics;

namespace WebAPI_Finder_Test.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        public string FullName { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime BirthDate { get; set; }

        [Index()]
        [StringLength(300)]
        public string Login { get; set; }

        public string AvatarImage { get; set; }

        [StringLength(500)]
        public string About { get; set; }

        public virtual ICollection<Audio> AudioTracks { get; set; }

        public int? CityId { get; set; }

        public City City { get; set; }

        public DateTime RegistrationDate { get; set; }

        public ICollection<Category> Categories { get; set; }

        public virtual SocialNetworks SocNetworks { get; set; }

        public ICollection<Video> Videos { get; set; }


        public ApplicationUser()
        {
            AudioTracks = new List<Audio>();
            Categories = new List<Category>();
            Videos = new List<Video>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("AzureDb", throwIfV1Schema: false)
        {
            this.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
        }

        public virtual DbSet<Audio> AudioTracks { get; set; }

        public virtual DbSet<Like> Likes { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Video> Videos { get; set; }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

      
    }
}
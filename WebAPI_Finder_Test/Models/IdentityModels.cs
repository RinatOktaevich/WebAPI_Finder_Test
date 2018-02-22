using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Finder_Test.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Login { get; set; }

        public string AvatarImage { get; set; }

        [StringLength(500)]
        public string About { get; set; }


        public virtual  ICollection<Audio> AudioTracks { get; set; }

        public ApplicationUser()
        {
            AudioTracks = new List<Audio>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }






    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DtConnnect", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Audio> AudioTracks { get; set; }

        public virtual DbSet<Like> Likes { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
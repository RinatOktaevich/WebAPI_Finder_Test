using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI_Finder_Test.Models
{
    public class Category
    {
        public Category()
        {
            //Users = new List<ApplicationUser>();
            //Audios = new List<Audio>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true, IsClustered = false)]
        public string Name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<Audio> Audios { get; set; }
    }
}
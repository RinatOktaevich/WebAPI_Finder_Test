using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI_Finder_Test.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string Url { get; set; }

        public string ApplicationUserId { get; set; }

        public  ApplicationUser User { get; set; }

        public Photo()
        {

        }

        public Photo(string url)
        {
            Url = url;
        }

    }
}
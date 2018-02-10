using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI_Finder_Test.Models
{
    public class Audio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType( DataType.Url)]
        [StringLength(128)]
        public string Url { get; set; }

        [Required]
        [StringLength(50)]
        public string Performer { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public  ApplicationUser User { get; set; }

        public Audio(string _url,string _pr,string _ttl)
        {
            Url = _url;
            Performer = _pr;
            Title = _ttl;
        }

    }
}
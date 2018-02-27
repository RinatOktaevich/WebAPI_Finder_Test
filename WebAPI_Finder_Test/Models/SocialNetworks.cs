using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI_Finder_Test.Models
{
    public class SocialNetworks
    {
        [Key,ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }


        public ApplicationUser ApplicationUser { get; set; }

        [RegularExpression(@"https://www.instagram.com/.{3,}")]
        [DataType(DataType.Url)]
        public string Instagram { get; set; }

        [DataType(DataType.Url)]
        public string Vk { get; set; }

        [DataType(DataType.Url)]
        public string FaceBook { get; set; }

        [DataType(DataType.Url)]
        public string YouTube { get; set; }

        [DataType(DataType.Url)]
        public string Twitter { get; set; }

    }
}
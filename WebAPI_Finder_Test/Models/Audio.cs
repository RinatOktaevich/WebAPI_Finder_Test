using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI_Finder_Test.Models
{
    public class Audio
    {
        #region Done

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [StringLength(128)]
        public string Url { get; set; }

        [Required]
        [StringLength(50)]
        public string Performer { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Audio(string _url, string _pr, string _ttl,string authLogin)
        {
            Url = _url;
            Performer = _pr;
            Title = _ttl;
            AuthorLogin = authLogin;
        }

        public Audio()
        {
            Likes = new List<Like>();
        }

        #endregion

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public string AuthorLogin { get; set; }

        [DataType(DataType.Url)]
        public string ImageCover { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

    }
}
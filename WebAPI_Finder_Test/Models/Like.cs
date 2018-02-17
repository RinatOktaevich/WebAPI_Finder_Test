using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI_Finder_Test.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AudioId { get; set; }
        public virtual Audio Audio { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; }


        public Like()
        {

        }

        public Like(int audioId,string  userId)
        {
            AudioId = audioId;
            ApplicationUserId = userId;
        }



    }
}
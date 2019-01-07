using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApps.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        [Display(Name = "Post")]
        public string PostText { get; set; }

        [Required]
        [Display(Name = "Author ID")]
        public string OwnerId { get; set; }

        [Required]
        [Display(Name = "Author")]
        public string OwnerUsername { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime DatePosted { get; set; }

        [Display(Name = "Comments")]
        public int NoOfComments { get; set; }
    }
}

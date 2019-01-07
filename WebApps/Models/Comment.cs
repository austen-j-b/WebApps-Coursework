using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApps.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string CommentText { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public string OwnerUserName { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        public int NoOfLikes { get; set; }

        [Required]
        public virtual Post ParentPost { get; set; }
    }
}

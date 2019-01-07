using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestingUsers.Models
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
        public string PostText { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        public int NoOfComments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApps.Models
{
    public class CreatePostViewModel
    {
        [Required, MaxLength(20)]
        public string Title { get; set; }
        [Required, MaxLength(500), Display(Name = "Your Post")]
        public string PostText { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApps.Models
{
    public class CreatePostViewModel
    {
        [Required, StringLength(500, MinimumLength = 1)]
        public string Title { get; set; }
        [Required, StringLength(500, MinimumLength = 1), Display(Name = "Your Post")]
        public string PostText { get; set; }
    }
}

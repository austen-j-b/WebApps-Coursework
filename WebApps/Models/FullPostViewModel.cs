﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApps.Models
{
    public class FullPostViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }

        public int PostID { get; set; }
        [Display(Name = "Author")]
        public string OwnerUserName { get; set; }
        public string Comment { get; set; }
    }
}

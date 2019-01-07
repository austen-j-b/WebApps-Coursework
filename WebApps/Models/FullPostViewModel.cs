using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApps.Models
{
    public class FullPostViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }

        public int PostID { get; set; }
        public string OwnerUserName { get; set; }
        public string Comment { get; set; }
    }
}

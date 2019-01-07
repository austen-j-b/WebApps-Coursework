using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApps.Models
{
    public class Like
    {
        //HAVE TO USE FLUENT API FOR COMPOSITE KEY
        public int CommentId { get; set; }
        public string LikerId { get; set; }
    }
}

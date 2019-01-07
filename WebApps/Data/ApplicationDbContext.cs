using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApps.Models;

namespace WebApps.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasKey(c => new { c.CommentId, c.LikerId });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<WebApps.Models.Post> Posts { get; set; }
        public DbSet<WebApps.Models.Comment> Comments { get; set; }
        public DbSet<WebApps.Models.Like> Likes { get; set; }
    }
}

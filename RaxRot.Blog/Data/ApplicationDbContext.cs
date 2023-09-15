﻿using Microsoft.EntityFrameworkCore;
using RaxRot.Blog.Models.Domain;

namespace RaxRot.Blog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
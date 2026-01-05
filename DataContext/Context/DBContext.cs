using DataContext.EntityConfiguration;
using DataContext.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataContext.Context
{
    public class DBContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<FileEntity> Files { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FileEntityConfiguration());

            base.OnModelCreating(builder);
        }
    }
}

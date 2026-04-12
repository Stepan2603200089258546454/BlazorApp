using DataContext.EntityConfiguration;
using DataContext.EntityConfiguration.Cloud;
using DataContext.Models;
using DataContext.Models.Cloud;
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
        public DbSet<PersonalCloud> PersonalClouds { get; set; }
        public DbSet<GlobalCloud> GlobalClouds { get; set; }
        public DbSet<CloudItem> CloudItems { get; set; }
        public DbSet<CloudFileData> CloudFileData { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FileEntityConfiguration());
            builder.ApplyConfiguration(new PersonalCloudConfiguration());
            builder.ApplyConfiguration(new GlobalCloudConfiguration());
            builder.ApplyConfiguration(new CloudItemConfiguration());
            builder.ApplyConfiguration(new CloudFileDataConfiguration());

            base.OnModelCreating(builder);
        }
    }
}

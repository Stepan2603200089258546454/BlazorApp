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
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
    }
}

using DataContext.Models.Cloud;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataContext.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<PersonalCloud> PersonalClouds { get; set; } = [];
        public List<GlobalCloud> GlobalClouds { get; set; } = [];
        public List<CloudItem> CloudItems { get; set; } = [];
    }
}

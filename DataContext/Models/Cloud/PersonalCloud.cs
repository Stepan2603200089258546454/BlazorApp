using System;
using System.Collections.Generic;
using System.Text;

namespace DataContext.Models.Cloud
{
    public class PersonalCloud
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<CloudItem> Items { get; set; } = [];
    }
}

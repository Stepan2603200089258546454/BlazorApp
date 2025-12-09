using System;
using System.Collections.Generic;
using System.Text;

namespace DataContext.Configuration
{
    public class DBConfiguration
    {
        public DBType Type { get; set; } = DBType.InMemory;
        public MemorySettings MemorySettings { get; set; } = new MemorySettings();
        public PostgreSettings? PostgreSettings { get; set; }
    }
}

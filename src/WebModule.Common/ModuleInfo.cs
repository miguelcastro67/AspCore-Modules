using System;
using System.Collections.Generic;
using System.Linq;

namespace WebModule
{
    public class ModuleInfo
    {
        public Type ModuleType { get; set; }
        public IEnumerable<string> Resources { get; set; }
    }
}

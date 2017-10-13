using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableMatching_CSharp
{
    public class Person
    {
        public GenderEnum Gender { get; set; }
        public string Name { get; set; }
        public List<string> SpousePreferenceNames { get; set; } 
        public bool IsMarried { get; set; }
        public string SpouseName { get; set; }
    }
}

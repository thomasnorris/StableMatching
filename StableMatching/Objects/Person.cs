using System.Collections.Generic;

namespace StableMatching.Objects
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

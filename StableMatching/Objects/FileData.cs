using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableMatching_CSharp
{
    public class FileData
    {
        public int NumPeoplePerGender { get; set; }
        public List<string> MenNames { get; set; }
        public List<string> WomanNames { get; set; }
        public List<Person> PeopleAndPreferences { get; set; }
    }
}

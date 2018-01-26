using System.Collections.Generic;

namespace StableMatching.Objects
{
    public class FileData
    {
        public int NumPeoplePerGender { get; set; }
        public List<string> MenNames { get; set; }
        public List<string> WomanNames { get; set; }
        public List<Person> PeopleAndPreferences { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableMatching_CSharp
{
    class Program
    {
        private const string FILE_NAME = "DataFile.txt";
        private const char SPACE_DELIMITER = ' ';
        private const char COLON_DELIMITER = ':';
        private static int _numPeoplePerGender;
        static void Main(string[] args)
        {
            var file = ReadFile();
            var womenGroup = file.PeopleAndPreferences.FindAll(m => m.Gender == GenderEnum.Female);
            var menGroup = file.PeopleAndPreferences.FindAll(m => m.Gender == GenderEnum.Male);

            RunMatching(menGroup, womenGroup);
            
        }

        private static void RunMatching(List<Person> proposingGroup, List<Person> proposeeGroup)
        {
            foreach (var person in proposingGroup)
            {
                var proposals = new List<Person>();
                while (!person.IsMarried)
                {
                    var foundMatch = false;
                    Person match = null;
                    while (!foundMatch)
                    {
                        match = proposeeGroup.Find(x => x.Name == person.SpousePreferenceNames.First());
                        if (!proposals.Contains(match))
                            foundMatch = true;
                        else
                            MoveFirstElementToEnd(person.SpousePreferenceNames);
                    }

                    if (!match.IsMarried)
                    {
                        person.IsMarried = true;
                        person.SpouseName = match.Name;
                        match.IsMarried = true;
                        match.SpouseName = person.Name;
                    }
                    else
                    {
                        var currentSpouseIndex = match.SpousePreferenceNames.IndexOf(match.SpouseName);
                        var potentialSpouseIndex = match.SpousePreferenceNames.IndexOf(person.Name);

                        if (potentialSpouseIndex < currentSpouseIndex)
                        {
                            var currentSpouse = proposingGroup.Find(m => m.Name == match.SpouseName);
                            currentSpouse.IsMarried = false;
                            currentSpouse.SpouseName = null;
                            person.IsMarried = true;
                            match.SpouseName = person.Name;
                        }
                        else
                            proposals.Add(match);
                    }
                }
            }

            if (!proposingGroup.All(m => m.IsMarried == true))
                RunMatching(proposingGroup, proposeeGroup);
        }

        private static void MoveFirstElementToEnd<T>(List<T> list) where T : class
        {
            var temp = list[0];
            list.Remove(temp);
            list.Add(temp);
        }

        private static FileData ReadFile()
        {
            var baseDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString());
            var fileLocation = string.Concat(baseDirectory, @"\", FILE_NAME);
            try
            {
                using (var sr = new StreamReader(fileLocation))
                {
                    var fileData = new FileData
                    {
                        NumPeoplePerGender = int.Parse(sr.ReadLine()),
                        MenNames = TrimAndConvertStringToList(sr.ReadLine(), SPACE_DELIMITER),
                        WomanNames = TrimAndConvertStringToList(sr.ReadLine(), SPACE_DELIMITER)
                    };
                    _numPeoplePerGender = fileData.NumPeoplePerGender;

                    var personList = new List<Person>();
                    PopulatePreferences(sr, personList, GenderEnum.Male);
                    PopulatePreferences(sr, personList, GenderEnum.Female);

                    fileData.PeopleAndPreferences = personList;

                    return fileData;
                }
            }
            catch (Exception ex)
            {
                ThrowAndExit(ex);
            }

            return new FileData();
        }

        private static void PopulatePreferences(StreamReader sr, List<Person> personList, GenderEnum gender)
        {
            sr.ReadLine();
            for (var i = 0; i < _numPeoplePerGender; i++)
            {
                var line = TrimAndConvertStringToList(sr.ReadLine(), COLON_DELIMITER);
                personList.Add(new Person
                {
                    Gender = gender,
                    Name = line[0],
                    SpousePreferenceNames = TrimAndConvertStringToList(line[1], SPACE_DELIMITER),
                    IsMarried = false
                });
            }
        }

        private static List<string> TrimAndConvertStringToList(string toConvert, char splitDelimiter)
        {
            return toConvert.Trim(SPACE_DELIMITER).Split(splitDelimiter).ToList();
        } 

        private static void ThrowAndExit(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to exit.");
            if (string.IsNullOrWhiteSpace(Console.ReadLine())) ;
                Environment.Exit(0);
        }
    }
}

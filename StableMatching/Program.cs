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
        private const string PROGRAM_NAME = "StableMatching.exe";
        private const char SPACE_DELIMITER = ' ';
        private const char COLON_DELIMITER = ':';
        private static int _numPeoplePerGender;

        static void Main(string[] args)
        {
            Console.Write("Starting " + PROGRAM_NAME + "\n\n");

            var file = ReadFile();
            var womenGroup = file.PeopleAndPreferences.FindAll(m => m.Gender == GenderEnum.Female);
            var menGroup = file.PeopleAndPreferences.FindAll(m => m.Gender == GenderEnum.Male);

            RunMatching(womenGroup, menGroup);
            ExitProg();
        }

        private static void RunMatching(List<Person> proposingGroup, List<Person> proposeeGroup)
        {
            foreach (var proposer in proposingGroup)
            {
                var proposals = new List<Person>();
                while (!proposer.IsMarried)
                {
                    var foundMatch = false;
                    Person match = null;
                    while (!foundMatch)
                    {
                        match = proposeeGroup.Find(x => x.Name == proposer.SpousePreferenceNames.First());
                        if (!proposals.Contains(match))
                            foundMatch = true;
                        else
                            // --Move the married person to the back of their preferences in order to grab the next one who might not be married
                            MoveFirstElementToEnd(proposer.SpousePreferenceNames);
                    }

                    if (!match.IsMarried)
                        MarkAsMarried(proposer, match);

                    else
                    {
                        // --Compare preference of current spouse and new spouse, booting the current spouse out if the potential spouse is a higher preference
                        var currentSpouseIndex = match.SpousePreferenceNames.IndexOf(match.SpouseName);
                        var potentialSpouseIndex = match.SpousePreferenceNames.IndexOf(proposer.Name);

                        if (potentialSpouseIndex < currentSpouseIndex)
                        {
                            var currentSpouse = proposingGroup.Find(m => m.Name == match.SpouseName);
                            currentSpouse.IsMarried = false;
                            currentSpouse.SpouseName = null;
                            MarkAsMarried(match, proposer);
                        }
                        else
                            proposals.Add(match);
                    }
                }
            }

            // --Recursively run through the program if not all of the proposing group is married (i.e. if someone had their spouse taken away from them)
            if (!proposingGroup.All(m => m.IsMarried == true))
                RunMatching(proposingGroup, proposeeGroup);
            else
                foreach (var person in proposingGroup)
                    Console.Write(person.Name + " is married to " + person.SpouseName + ".\n");
        }

        private static void MarkAsMarried(Person person1, Person person2)
        {
            person1.IsMarried = true;
            person1.SpouseName = person2.Name;
            person2.IsMarried = true;
            person2.SpouseName = person1.Name;
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
                // --Catches and file reading exceptions and logs them to the console instead of crashing
                ExitProg(ex);
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

        private static void ExitProg(Exception ex = null)
        {
            if (ex != null)
                Console.WriteLine(ex.Message);

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}

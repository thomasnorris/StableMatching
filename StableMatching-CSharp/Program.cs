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
                    Gender = gender.ToString(),
                    Name = line[0],
                    Preferences = TrimAndConvertStringToList(line[1], SPACE_DELIMITER)
                });
            }
        }

        private static List<string> TrimAndConvertStringToList(string toConvert, char delimiter)
        {
            return toConvert.Trim(SPACE_DELIMITER).Split(delimiter).ToList();
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

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
                var scanner = new StreamReader(fileLocation);
            } 
            catch (Exception ex)
            {
                ThrowAndExit(ex);
            }

            return new FileData();
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Ddnd
{
    public class Program
    {
        //imagined program usage ddnd [command] [commandArg1] [commandArg2] ... [commandArgN]
        public static void Main(string[] arrayArgs)
        {
            List<string> args = arrayArgs.ToList();

            string commandArg = args.Count >= 1 ? args[0] : null;

            if (string.IsNullOrEmpty(commandArg))
            {
                Console.WriteLine("No valid command argument provided");
            }
            else
            {
                List<string> commandArgs = args.Count >= 2 ? args.GetRange(1, args.Count) : new List<string>();

                switch (commandArg.ToLower())
                {
                    case "roll":
                        Roll(commandArgs);
                        break;
                    case "getname":
                        GetName();
                        break;
                    case "getfullname":
                        GetFullName();
                        break;
                    case "getrandomname":
                        GetRandomName();
                        break;
                    default:
                        Console.WriteLine(commandArg + " is not a support command");
                        break;
                }
            }
        }
        
        private static void Roll(List<string> args)
        {

        }

        private static void GetName()
        {   
            using (StreamReader r = new StreamReader("./json/names.json"))
            {
                string namesJson = r.ReadToEnd();
                List<string> namesList = JsonConvert.DeserializeObject<RootNamesJson>(namesJson).Names;

                Random random = new Random();
                int randomIndex = random.Next(0, namesList.Count - 1);

                Console.WriteLine(namesList[randomIndex]);
            }
        }        

        private static void GetFullName()
        {
            using (StreamReader r = new StreamReader("./json/names.json"))
            {
                string namesJson = r.ReadToEnd();
                List<string> namesList = JsonConvert.DeserializeObject<RootNamesJson>(namesJson).Names;

                Random random = new Random();
                int randomIndex1 = random.Next(0, namesList.Count - 1);
                int randomIndex2 = random.Next(0, namesList.Count - 1);

                Console.WriteLine(namesList[randomIndex1] + " " + namesList[randomIndex2]);
            }
        }

        public class RootNamesJson
        {
            public List<string> Names { get; set; }
        }

        private static void GetRandomName()
        {
            List<string> vowels = new List<string>() { "a", "e", "i", "o", "u", "y" };
            List<string> consonants = new List<string>() { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };

            Random random = new Random();

            int numSyllables = random.Next(10) + 1;//1-10 sylls

            string name = "";

            bool mustHaveStartConsonant = random.NextBool();
            bool mustHaveEndConsonant = random.NextBool();

            for(int i = 0; i < numSyllables; i++)
            {
                bool hasStartingConsonant = mustHaveStartConsonant ? mustHaveStartConsonant : random.NextBool();
                bool hasEndingConsonant = mustHaveEndConsonant ? mustHaveEndConsonant : random.NextBool();
                string syll = "";

                if (hasStartingConsonant)
                {
                    string randomCons = consonants[random.Next(consonants.Count)];
                    syll += randomCons;
                }
                string randomVowel = vowels[random.Next(vowels.Count)];
                syll += randomVowel;
                if (hasEndingConsonant)
                {
                    string randomCons = consonants[random.Next(consonants.Count)];
                    syll += randomCons;
                }

                name += syll;

                //set consonant flags for next syllable
                mustHaveStartConsonant = !hasEndingConsonant;
                mustHaveEndConsonant = random.NextBool();
            }

            Console.WriteLine(name);
            return;
        }

    }
}

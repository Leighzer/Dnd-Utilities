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
                    case "getmonster":
                        GetMonster();
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

        public static void GetMonster()
        {
            Random random = new Random();

            StreamReader monsterReader = new StreamReader("./json/monsters.json");
            string monsterJson = monsterReader.ReadToEnd();
            List<string> monsterList = JsonConvert.DeserializeObject<RootMonstersJson>(monsterJson).Monsters;
            monsterReader.Dispose();

            StreamReader adjectivesReader = new StreamReader("./json/adjectives.json");
            string adjectiveJson = adjectivesReader.ReadToEnd();
            List<string> adjectiveList = JsonConvert.DeserializeObject<RootAdjectivesJson>(adjectiveJson).Adjectives;
            adjectivesReader.Dispose();


            List<int> legOptions = new List<int>() { 0, 2, 4, 6, 8 };

            int numLegs = legOptions[random.Next(legOptions.Count)];

            bool hasWings = random.NextDouble() > 0.9;

            List<int> armOptions = new List<int>() { 0, 2, 4 };

            int numArms = armOptions[random.Next(armOptions.Count)];

            int numAdjectives = random.Next(3) + 1;
            List<string> adjectives = new List<string>();
            for(int i = 0; i < numAdjectives; i++)
            {
                bool isDone = false;
                while (!isDone)
                {
                    string candidateAdjective = adjectiveList[random.Next(adjectiveList.Count())];
                    if (!adjectives.Contains(candidateAdjective))
                    {
                        adjectives.Add(candidateAdjective);
                        isDone = true;
                    }
                }
            }

            string monsterType = monsterList[random.Next(monsterList.Count)];

            string monsterString = "";

            if (hasWings)
            {
                monsterString += "winged ";
            }

            if(numArms > 0)
            {
                monsterString += $"{numArms} armed ";
            }
            else
            {
                monsterString += "armless ";
            }

            if(numLegs > 0)
            {
                monsterString += $"{numLegs} legged ";
            }
            else
            {
                monsterString += "slithering ";
            }

            for(int i = 0; i < adjectives.Count; i++)
            {
                monsterString += $"{adjectives[i]} ";
            }

            monsterString += $"{monsterType} ";

            Console.WriteLine(monsterString);

            return;
        }

        public class RootMonstersJson
        {
            public List<string> Monsters { get; set; }
        }

        public class RootAdjectivesJson
        {
            public List<string> Adjectives { get; set; }
        }
    }
}

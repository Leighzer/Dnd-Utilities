﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace Ddnd
{
    // imagined program usage ddnd [command] [commandArg1] [commandArg2] ... [commandArgN]
    public class Program
    {
        private static readonly string appDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath);

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
                List<string> commandArgs = args.Count >= 2 ? args.GetRange(1, args.Count - 1) : new List<string>();

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
                    case "getrandomfullname":
                        GetRandomFullName();
                        break;
                    case "getmonster":
                        GetMonster();
                        break;
                    default:
                        Console.WriteLine(commandArg + " is not a supported command");
                        break;
                }
            }
        }
        
        private static void Roll(List<string> args)
        {
            Random random = new Random();

            for(int i = 0; i < args.Count; i++)
            {
                string arg = args[i];

                var split = arg.Split('d');
                

                if(split.Length == 2)
                {
                    var rollsString = split[0];
                    var facesString = split[1];

                    int numRolls;
                    int numFaces;

                    if (string.IsNullOrWhiteSpace(rollsString))
                    {
                        numRolls = 1;
                    }
                    else
                    {
                        if (!int.TryParse(rollsString, out numRolls))
                        {
                            Console.WriteLine($"{rollsString} is not a valid number of rolls - dice skipped");
                            continue;
                        }
                    }
                    
                    if(!int.TryParse(facesString,out numFaces))
                    {
                        Console.WriteLine($"{facesString} is not a valid number of faces - dice skipped");
                        continue;
                    }

                    Console.Write($"{numRolls} roll(s) of a d{numFaces} yields: ");
                    for(int j = 0; j < numRolls; j++)
                    {
                        int rollResult = random.Next(numFaces) + 1;
                        Console.Write($"{rollResult} ");
                    }
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($"{arg} is not a valid roll argument");
                }
            }

        }

        private static void GetName()
        {   
            using (StreamReader r = new StreamReader(appDir + "/json/names.json"))
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
            using (StreamReader r = new StreamReader(appDir + "/json/names.json"))
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
            string name = getRandomName();
            Console.WriteLine(name);
            return;
        }

        private static string getRandomName()
        {
            List<string> vowels = new List<string>() { "a", "e", "i", "o", "u" };
            List<string> consonants = new List<string>() { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };

            Random random = new Random();

            int numSyllables = random.Next(10) + 1;//1-10 sylls

            string name = "";

            bool mustHaveStartConsonant = random.NextBool();
            bool mustHaveEndConsonant = random.NextBool();

            for (int i = 0; i < numSyllables; i++)
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

                // set consonant flags for next syllable
                mustHaveStartConsonant = !hasEndingConsonant;
                mustHaveEndConsonant = random.NextBool();
            }

            return name;
        }

        public static void GetRandomFullName()
        {
            string firstName = getRandomName();
            string lastName = getRandomName();

            Console.WriteLine(firstName + " " + lastName);
            return;
        }

        public static void GetMonster()
        {
            Random random = new Random();

            StreamReader monsterReader = new StreamReader(appDir + "/json/monsters.json");
            string monsterJson = monsterReader.ReadToEnd();
            List<string> monsterList = JsonConvert.DeserializeObject<RootMonstersJson>(monsterJson).Monsters;
            monsterReader.Dispose();

            StreamReader adjectivesReader = new StreamReader(appDir + "/json/adjectives.json");
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

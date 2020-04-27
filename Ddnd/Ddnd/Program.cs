using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                if(commandArg.ToLower() == "roll")
                {
                    if (args.Count >= 2)
                    {
                        Roll(args.GetRange(1,args.Count));
                    }
                    else
                    {
                        Console.WriteLine("Not enough arguments provided to command roll");
                    }
                }

                if(commandArg.ToLower() == "getname")
                {
                    GetName();
                }

                if(commandArg.ToLower() == "getfullname")
                {
                    GetFullName();
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

    }
}

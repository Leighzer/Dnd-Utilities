using System;
using System.Collections.Generic;
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
            }

        }

        //Roll command
        //command parse and roll code ehre
        private static void Roll(List<string> args)
        {

        }
    }
}

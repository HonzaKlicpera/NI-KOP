using System;
using System.Collections.Generic;
using System.IO;
using KnapsackProblem.Common;
using KnapsackProblem.DecisionVersion;
using KnapsackProblem.Helpers;
using CommandLine;

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(
                o =>
                {
                    Console.WriteLine(o.ProblemVersion);
                });

            /*
            bool checkValidity = true;
            Dictionary<int, int> correctAnswers = new Dictionary<int, int>();
            IDecisionKnapsackStrategy strategy = new DecisionBruteForceStrategy();

            if (checkValidity)
            {
                correctAnswers = ParseCorrectAnswers("C:\\Downloads\\NR\\NK20_sol.dat");
            }
            using (StreamReader file = new StreamReader("C:\\Downloads\\NR\\NR20_inst.dat"))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var dec = new DecisionKnapsackInstance(ln);
                    var result = dec.DoesSolutionExist(strategy);
                    Console.WriteLine(dec.Id + " " + result);

                    if (checkValidity)
                    {
                        if (dec.MinimalPrice <= correctAnswers[dec.Id] == result)
                            Console.WriteLine("All good!");
                        else
                            Console.WriteLine("noooooo");
                    }
                }
                
            }
            */
        }

        static Dictionary<int, int> ParseCorrectAnswers(string location)
        {
            var correctAnswers = new Dictionary<int, int>();
            using (StreamReader file = new StreamReader(location))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var inputFields = ln.Split(' ');
                    correctAnswers[int.Parse(inputFields[0])] = int.Parse(inputFields[2]);
                }
            }
            return correctAnswers;
        }


    }

}

using System;
using System.Collections.Generic;
using System.IO;
using KnapsackProblem.Common;
using KnapsackProblem.DecisionVersion;
using KnapsackProblem.Helpers;
using CommandLine;
using KnapsackProblem.Exceptions;
using KnapsackProblem.ConstructiveVersion.Strategies;
using KnapsackProblem.ConstructiveVersion;

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(
                o => ProcessArguments(o));
        }

        static void ProcessArguments(CommandLineOptions options)
        {
            try
            {
                if (options.ProblemVersion.Equals("decision", StringComparison.OrdinalIgnoreCase)
                    || options.ProblemVersion.Equals("d", StringComparison.OrdinalIgnoreCase))
                {
                    new DecisionVersionHandler().HandleData(options);
                }
                else if(options.ProblemVersion.Equals("constructive", StringComparison.OrdinalIgnoreCase)
                    || options.ProblemVersion.Equals("c", StringComparison.OrdinalIgnoreCase))
                {
                    new ConstructiveVersionHandler().HandleData(options);
                }
                else throw new InvalidArgumentException($"{options.ProblemVersion} is not a valid knapsack problem version. " +
                    $"Valid versions:\n {ProblemVersions()}");

            }
            catch (InvalidArgumentException e)
            {
                Console.WriteLine($"Invalid argument: {e.Message}");
            }
        }

        static string ProblemVersions()
        {
            return "Decision \n" +
                "Constructive";
        }
    }
}

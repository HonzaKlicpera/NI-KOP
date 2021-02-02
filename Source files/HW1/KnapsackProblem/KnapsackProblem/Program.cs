using System;
using System.Collections.Generic;
using System.IO;
using KnapsackProblem.Common;
using KnapsackProblem.DecisionVersion;
using KnapsackProblem.Helpers;
using CommandLine;
using KnapsackProblem.Exceptions;

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
                    ProcessDecisionVersion(options);
                }
                else throw new InvalidArgumentException($"{options.ProblemVersion} is not a valid knapsack problem version. " +
                    $"Valid versions:\n {ProblemVersions()}");

            }
            catch (InvalidArgumentException e)
            {
                Console.WriteLine($"Invalid argument: {e.Message}");
            }
        }

        static void ProcessDecisionVersion(CommandLineOptions options)
        {
            IList<DecisionKnapsackInstance> instances;

            //Open and parse the input instances
            Console.WriteLine($"Opening file {options.InputFile}");
            try
            {
                instances = InputReader.ReadDecisionKnapsackInstances(options.InputFile);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Could not open input instances file at {options.InputFile} ({e.Message})");
                return;
            }
            catch (InvalidInputFormatException e)
            {
                Console.WriteLine($"Could not parse the input instances file: {e.Message}");
                return;
            }

            //Solve the input instances
            var strategy = GetDecisionStrategy(options.Strategy);
            var solutions = strategy.SolveAll(instances, options.Strategy, options.DataSetName);

            //Compare with the reference solution if specified
            if (options.ReferenceFile != null)
            {
                Console.WriteLine("Validating the results...");
                try
                {
                    var referenceSolutions = InputReader.ReadReferenceSolutions(options.ReferenceFile);
                    SolutionValidator.ValidateDecisionSolutions(solutions, referenceSolutions);
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Could not open reference instances file at {options.ReferenceFile} ({e.Message})");
                    return;
                }
                catch (InvalidInputFormatException e)
                {
                    Console.WriteLine($"Could not parse the reference instances file: {e.Message}");
                    return;
                }
            }

            //Output the solution
            try
            {
                OutputWriter.WriteAllResults(solutions, options.OutputFile);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error while writing into the output file: {e.Message}");
            }
        }

        static DecisionStrategy GetDecisionStrategy(string strategyField)
        {
            if (strategyField.Equals("BruteForce", StringComparison.OrdinalIgnoreCase))
                return new DecisionBruteForce();
            else if (strategyField.Equals("BranchAndBound", StringComparison.OrdinalIgnoreCase))
                return new DecisionBranchBound();
            else if (strategyField.Equals("BranchAndBoundSortedWeight", StringComparison.OrdinalIgnoreCase))
                return new DecisionBranchBoundSortedWeight();
            else if (strategyField.Equals("BranchAndBoundSortedPrice", StringComparison.OrdinalIgnoreCase))
                return new DecisionBranchBoundSortedPrice();
            else if (strategyField.Equals("BranchAndBoundSortedBoth", StringComparison.OrdinalIgnoreCase))
                return new DecisionBranchBoundSortedBoth();
            throw new InvalidArgumentException($"{strategyField} is not a valid strategy for decision version. Valid strategies: " +
                $"\n {DecisionVersionStrategies()}");
        }

        static string DecisionVersionStrategies()
        {
            return "BruteForce \n " +
                "BranchAndBound \n " +
                "BranchAndBoundSortedWeight \n " +
                "BranchAndBoundSortedPrice \n " +
                "BranchAndBoundSortedBoth";
        }

        static string ProblemVersions()
        {
            return "Decision";
        }
    }
}

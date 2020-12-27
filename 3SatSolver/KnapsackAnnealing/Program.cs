using CommandLine;
using KnapsackAnnealing.Common;
using KnapsackAnnealing.Solver;
using KnapsackProblem.Common;
using KnapsackProblem.Exceptions;
using KnapsackProblem.Helpers;
using System;
using System.Collections.Generic;
using System.IO;


namespace KnapsackAnnealing
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(
                o => ProcessArguments(o));
        }

        static void ProcessArguments(CommandLineOptions cmdLineOptions)
        {
            IList<KnapsackResult> results = new List<KnapsackResult>();
            try
            {
                //Process and load input instances
                var instances = InputReader.ReadKnapsackInstances(cmdLineOptions.InputFile);
                /*
                var annealingOptions = GetAnnealingOptions(cmdLineOptions);
                foreach (var instance in instances) {
                    var solver = new SimulatedAnnealingSolver(instance, annealingOptions);
                    results.Add(solver.Solve());
                }

                //var results = strategy.SolveAll(instances, options.Strategy, options.DataSetName);
                */
                results = PerformanceTester.SolveWithPerformanceTest(instances);
            }
            catch (InvalidArgumentException e)
            {
                Console.WriteLine($"Invalid argument: {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Could not open input instances file at {cmdLineOptions.InputFile} ({e.Message})");
            }
            catch (InvalidInputFormatException e)
            {
                Console.WriteLine($"Could not parse the input instances file: {e.Message}");
            }

            //Output the solution
            try
            {
                OutputWriter.WriteConstructiveResults(results, cmdLineOptions.OutputFile);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error while writing into the output file: {e.Message}");
            }
        }

        
    }
}

using CommandLine;
using KnapsackAnnealing.Common;
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

        static void ProcessArguments(CommandLineOptions options)
        {
            IList<KnapsackResult> results = null;
            try
            {
                //Process and load input instances
                var instances = InputReader.ReadKnapsackInstances(options.InputFile);
                var annealingOptions = GetAnnealingOptions(options);

                //var results = strategy.SolveAll(instances, options.Strategy, options.DataSetName);
                //results = PerformanceTester.SolveWithPerformanceTest(instances, options);
            }
            catch (InvalidArgumentException e)
            {
                Console.WriteLine($"Invalid argument: {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Could not open input instances file at {options.InputFile} ({e.Message})");
            }
            catch (InvalidInputFormatException e)
            {
                Console.WriteLine($"Could not parse the input instances file: {e.Message}");
            }

            //Output the solution
            try
            {
                OutputWriter.WriteConstructiveResults(results, options.OutputFile);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error while writing into the output file: {e.Message}");
            }
        }

        static AnnealingOptions GetAnnealingOptions(CommandLineOptions commandLineOptions)
        {
            var annealingOptions = new AnnealingOptions();


            return annealingOptions;
        }
    }
}

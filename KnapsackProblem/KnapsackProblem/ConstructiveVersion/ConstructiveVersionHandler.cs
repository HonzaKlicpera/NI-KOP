using KnapsackProblem.Common;
using KnapsackProblem.ConstructiveVersion.Strategies;
using KnapsackProblem.Exceptions;
using KnapsackProblem.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion
{
    class ConstructiveVersionHandler: AbstractVersionHandler<ConstructiveResult, KnapsackInstance>
    {
        public override void HandleData(CommandLineOptions options)
        {
            //Process and load input instances
            if (!ProcessInputInstances(out var instances, options.InputFile, InputFieldParser.ParseConstructiveKnapsackInstance))
                return;

            var strategy = GetConstructiveStrategy(options.Strategy);
            //var results = strategy.SolveAll(instances, options.Strategy, options.DataSetName);
            var results = PerformanceTester.SolveWithPerformanceTest(instances, strategy, options);

            //Compare with the reference solution if specified
            if (options.ReferenceFile != null)
            {
                if (!ValidateResults<ConstructiveResult, KnapsackInstance>(results, options.ReferenceFile, SolutionValidator.ConstructiveComparator))
                    return;
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

        private ConstructiveStrategy GetConstructiveStrategy(string strategyField)
        {
            if (strategyField.Equals("BruteForce", StringComparison.OrdinalIgnoreCase))
                return new ConstructiveBruteForce();
            else if (strategyField.Equals("BranchAndBound", StringComparison.OrdinalIgnoreCase))
                return new ConstructiveBranchBound();
            else if (strategyField.Equals("DPCapacity", StringComparison.OrdinalIgnoreCase))
                return new ConstructiveDPCapacity();
            else if (strategyField.Equals("DPPrice", StringComparison.OrdinalIgnoreCase))
                return new ConstructiveDPPrice();
            throw new InvalidArgumentException($"{strategyField} is not a valid strategy for decision version. Valid strategies: " +
                $"\n {ConstructiveVersionStrategies()}");
        }

        static string ConstructiveVersionStrategies()
        {
            return "BruteForce \n " +
                "BranchAndBound \n ";
        }
    }
}

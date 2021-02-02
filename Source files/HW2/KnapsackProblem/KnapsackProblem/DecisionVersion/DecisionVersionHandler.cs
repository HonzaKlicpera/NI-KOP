using KnapsackProblem.Common;
using KnapsackProblem.Exceptions;
using KnapsackProblem.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionVersionHandler: AbstractVersionHandler<DecisionResult, DecisionKnapsackInstance>
    {
        public override void HandleData(CommandLineOptions options)
        {
            //Process and load input instances
            if (!ProcessInputInstances(out var instances, options.InputFile, InputFieldParser.ParseDecisionKnapsackInstance))
                return;

            //Solve the input instances
            var strategy = GetDecisionStrategy(options.Strategy);
            var results = strategy.SolveAll(instances, options.Strategy, options.DataSetName);

            //Compare with the reference solution if specified
            if (options.ReferenceFile != null)
            {
                if (!ValidateResults<DecisionResult, DecisionKnapsackInstance>(results, options.ReferenceFile, SolutionValidator.DecisionComparator))
                    return;
            }


            //Output the solution
            try
            {
                OutputWriter.WriteDecisionResults(results, options.OutputFile);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error while writing into the output file: {e.Message}");
            }
        }

        private DecisionStrategy GetDecisionStrategy(string strategyField)
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


        private string DecisionVersionStrategies()
        {
            return "BruteForce \n " +
                "BranchAndBound \n " +
                "BranchAndBoundSortedWeight \n " +
                "BranchAndBoundSortedPrice \n " +
                "BranchAndBoundSortedBoth";
        }
    }
}

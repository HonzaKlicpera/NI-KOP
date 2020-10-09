using KnapsackProblem.Common;
using KnapsackProblem.ConstructiveVersion;
using KnapsackProblem.DecisionVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem.Helpers
{
    public static class SolutionValidator
    {

        public static void ValidateResults<R, I>(IList<R> results, Dictionary<int, KnapsackReferenceSolution> referenceSolutions, Func<R, KnapsackReferenceSolution, bool> comparator)
            where I: KnapsackInstance where R: AbstractResult<I>
        {
            bool allCorrect = true;
            foreach (var r in results)
            {
                var referenceSolution = referenceSolutions[r.KnapsackInstance.Id];

                if (!comparator(r, referenceSolution))
                    allCorrect = false;
            }

            if (allCorrect)
                Console.WriteLine("All solutions match the reference data");
            else
                Console.WriteLine("Not all solutions match the reference data, see the log above for more details");
        }

        public static bool DecisionComparator(DecisionResult result, KnapsackReferenceSolution referenceSolution)
        {
            var shouldPermutationExist = referenceSolution.Price >= result.KnapsackInstance.MinimalPrice;
            if (result.PermutationExists != shouldPermutationExist) 
            {
                Console.WriteLine($"Permutation instance solution (id {result.KnapsackInstance.Id}) incorrect," +
                            $" result: {result.PermutationExists}, expected result: {shouldPermutationExist}");
                return false;
            }
            return true;
        }

        public static bool ConstructiveComparator(ConstructiveResult result, KnapsackReferenceSolution referenceSolution)
        {
            if (!result.Solution.SolutionVector.SequenceEqual(referenceSolution.SolutionVector))
            {
                Console.WriteLine($"Permutation instance solution (id {result.KnapsackInstance.Id}) incorrect," +
                    $" result: {result.Solution.SolutionVector.Aggregate(new StringBuilder(), Aggregator)}," +
                    $" expected result: {referenceSolution.SolutionVector.Aggregate(new StringBuilder(), Aggregator)}");
                return false;
            }
            return true;
        }

        private static StringBuilder Aggregator(StringBuilder acc, bool current)
        {
            acc.Append(current ? "1" : "0");
            acc.Append(",");
            return acc;
        }
    }
}

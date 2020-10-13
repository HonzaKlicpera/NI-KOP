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
            result.ReferencePriceDiff = referenceSolution.Price - result.Configuration.Price;
            if (result.Configuration.Price != referenceSolution.Price || result.Configuration.Weight > result.KnapsackInstance.KnapsackSize)
            {
                Console.WriteLine($"Permutation instance solution (id {result.KnapsackInstance.Id}) incorrect," +
                    $" result: {OutputWriter.ItemVectorToString(result.Configuration.ItemVector)} (price {result.Configuration.Price})" +
                    $" expected result: {OutputWriter.ItemVectorToString(referenceSolution.ItemVector)} (price {referenceSolution.Price})");
                return false;
            }
            return true;
        }

    }
}

using KnapsackProblem.Common;
using KnapsackProblem.DecisionVersion;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Helpers
{
    public static class SolutionValidator
    {
        public static void ValidateDecisionSolutions(IList<DecisionSolution> solutions, Dictionary<int, KnapsackReferenceSolution> referenceSolutions)
        {
            bool allCorrect = true;
            foreach (var s in solutions)
            {
                var referenceSolution = referenceSolutions[s.KnapsackInstance.Id];
                var shouldPermutationExist = referenceSolution.Price >= s.KnapsackInstance.MinimalPrice;

                if (s.PermutationExists != shouldPermutationExist)
                {
                    Console.WriteLine($"Permutation instance solution (id {s.KnapsackInstance.Id}) incorrect," +
                        $" result: {s.PermutationExists}, expected result: {shouldPermutationExist}");
                    allCorrect = false;
                }
            }
            if (allCorrect)
                Console.WriteLine("All solutions match the reference data");
            else
                Console.WriteLine("Not all solutions match the reference data, see the log above for more details");
        }
    }
}

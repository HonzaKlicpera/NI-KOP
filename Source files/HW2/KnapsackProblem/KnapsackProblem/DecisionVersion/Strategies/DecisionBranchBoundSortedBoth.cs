using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using KnapsackProblem.Common;

namespace KnapsackProblem.DecisionVersion
{
    class DecisionBranchBoundSortedBoth : DecisionStrategy
    {
        
        public override DecisionResult Solve(DecisionKnapsackInstance instance)
        {
            var priceSum = instance.GetPriceOfAllItems();
            if (instance.MinimalPrice > priceSum)
                return new DecisionResult { KnapsackInstance = instance, NumberOfSteps = 1, PermutationExists = false };
            if (instance.MinimalPrice == 0)
                return new DecisionResult { KnapsackInstance = instance, NumberOfSteps = 1, PermutationExists = true };

            var priceRange = Math.Max(0.01, priceSum - instance.MinimalPrice);
            //the list is being processed backwards in the algorithm, so it must be sorted in ascending order
            instance.Items = instance.Items.OrderBy(a => Math.Max(a.Weight / instance.KnapsackSize, a.Price / priceRange)).
                ToList();
            
            return new DecisionBranchBound().Solve(instance);
        }
    }
}

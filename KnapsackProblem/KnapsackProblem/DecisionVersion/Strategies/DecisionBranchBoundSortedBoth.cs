using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using KnapsackProblem.Common;

namespace KnapsackProblem.DecisionVersion
{
    class DecisionBranchBoundSortedBoth : DecisionStrategy
    {
        
        public override DecisionSolution Solve(DecisionKnapsackInstance instance)
        {
            var priceSum = instance.GetPriceOfAllItems();
            if (instance.MinimalPrice > priceSum)
                return new DecisionSolution { KnapsackInstance = instance, NumberOfSteps = 1, PermutationExists = false };
            if (instance.MinimalPrice == 0)
                return new DecisionSolution { KnapsackInstance = instance, NumberOfSteps = 1, PermutationExists = true };

            var priceRange = Math.Max(0.01, priceSum - instance.MinimalPrice);

            instance.Items = instance.Items.OrderBy(a =>
                        Math.Max(Math.Min(1.0, a.Weight / instance.KnapsackSize),
                        Math.Min(1.0, a.Price / priceRange))).
                ToList();
            
            return new DecisionBranchBound().Solve(instance);
        }
    }
}

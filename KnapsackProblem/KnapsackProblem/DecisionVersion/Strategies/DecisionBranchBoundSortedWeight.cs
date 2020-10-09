using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionBranchBoundSortedWeight: DecisionStrategy
    {
        public override DecisionResult Solve(DecisionKnapsackInstance instance)
        {
            //the list is being processed backwards in the algorithm, so it must be sorted in ascending order
            instance.Items = instance.Items.OrderBy(a => a.Weight).ToList();

            return new DecisionBranchBound().Solve(instance);
        }
    }
}

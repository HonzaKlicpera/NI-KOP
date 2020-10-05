using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionBranchBoundSortedWeight: DecisionStrategy
    {
        public override DecisionSolution Solve(DecisionKnapsackInstance instance)
        {
            instance.Items = instance.Items.OrderBy(a => a.Weight).ToList();

            return new DecisionBranchBound().Solve(instance);
        }
    }
}

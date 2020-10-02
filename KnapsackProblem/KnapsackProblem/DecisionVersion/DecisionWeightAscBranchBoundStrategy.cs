using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.DecisionVersion
{
    class DecisionWeightAscBranchBoundStrategy: DecisionStrategy
    {
        public override DecisionSolution Solve(DecisionKnapsackInstance instance)
        {
            instance.Items = instance.Items.OrderBy(a => a.Weight).ToList();

            return new DecisionBranchBoundStrategy().Solve(instance);
        }
    }
}

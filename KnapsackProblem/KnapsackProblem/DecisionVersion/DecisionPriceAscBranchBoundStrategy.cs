using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.DecisionVersion
{
    class DecisionPriceAscBranchBoundStrategy : DecisionStrategy
    {
        
        public override DecisionSolution Solve(DecisionKnapsackInstance instance)
        {
            instance.Items = instance.Items.OrderBy(a => a.Price).ToList();

            return new DecisionBranchBoundStrategy().Solve(instance);
        }
    }
}

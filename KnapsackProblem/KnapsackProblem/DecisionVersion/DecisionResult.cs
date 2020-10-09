using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionResult: AbstractResult<DecisionKnapsackInstance>
    {
        public bool PermutationExists { get; set; }
    }
}

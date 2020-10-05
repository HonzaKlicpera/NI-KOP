using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionSolution
    {
        public ulong NumberOfSteps { get; set; }

        public bool PermutationExists { get; set; }

        public string Strategy { get; set; }

        public string DataSetName { get; set; }

        public DecisionKnapsackInstance KnapsackInstance { get; set; }
    }
}

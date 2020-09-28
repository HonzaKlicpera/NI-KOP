using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public abstract class DecisionStrategy
    {
        public int NumberOfSteps { get; protected set; }

        public abstract bool DoesSolutionExist(DecisionKnapsackInstance decisionVersion);
    }
}

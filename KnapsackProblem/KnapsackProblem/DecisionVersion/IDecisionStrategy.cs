using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public interface IDecisionStrategy
    {
        public bool DoesSolutionExist(DecisionKnapsackInstance decisionVersion);
    }
}

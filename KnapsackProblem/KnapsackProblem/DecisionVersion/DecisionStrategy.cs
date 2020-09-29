using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public abstract class DecisionStrategy
    {
        protected int numberOfSteps;

        public abstract DecisionSolution Solve(DecisionKnapsackInstance instance);

        public IList<DecisionSolution> SolveAll(IList<DecisionKnapsackInstance> instances)
        {
            var solutions = new List<DecisionSolution>();
            foreach (var instance in instances)
            {
                solutions.Add(Solve(instance));
            }

            return solutions;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public abstract class DecisionStrategy
    {
        protected ulong numberOfSteps;

        public abstract DecisionSolution Solve(DecisionKnapsackInstance instance);

        public IList<DecisionSolution> SolveAll(IList<DecisionKnapsackInstance> instances)
        {
            var solutions = new List<DecisionSolution>();
            foreach (var instance in instances)
            {
                Console.WriteLine($"Processing instance no. {instance.Id}");
                solutions.Add(Solve(instance));
            }

            return solutions;
        }
    }
}

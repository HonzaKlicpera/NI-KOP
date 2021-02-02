using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.DecisionVersion
{
    public abstract class DecisionStrategy
    {
        protected ulong numberOfSteps;

        public abstract DecisionSolution Solve(DecisionKnapsackInstance instance);

        public IList<DecisionSolution> SolveAll(IList<DecisionKnapsackInstance> instances, string strategy, string dataSetName)
        {
            var solutions = new List<DecisionSolution>();
            foreach (var instance in instances)
            {
                //Console.WriteLine($"Processing instance no. {instance.Id}");
                var solution = Solve(instance);
                solution.Strategy = strategy;
                solution.DataSetName = dataSetName;
                solutions.Add(solution);
            }

            return solutions;
        }
    }
}

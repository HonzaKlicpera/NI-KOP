using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Common
{
    public abstract class AbstractStrategy<R, I> where R : AbstractResult<I> where I : KnapsackInstance
    {
        protected ulong numberOfSteps;

        public abstract R Solve(I instance);

        public IList<R> SolveAll(IList<I> instances, string strategy, string dataSetName)
        {
            var results = new List<R>();
            foreach (var instance in instances)
            {
                Console.WriteLine($"Processing instance no. {instance.Id}");
                var result = Solve(instance);
                result.Strategy = strategy;
                result.DataSetName = dataSetName;
                results.Add(result);
            }

            return results;
        }
    }
}

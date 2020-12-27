using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public abstract class ConstructiveStrategy: AbstractStrategy<ConstructiveResult, KnapsackInstance>
    {
        protected KnapsackConfiguration BestConfiguration;

        public abstract void FreeAll();

        protected IList<bool> CreateEmptySolution(int n)
        {
            var emptySolution = new List<bool>();
            for(int i = 0; i < n; i++)
            {
                emptySolution.Add(false);
            }
            return emptySolution;
        }
    }
}

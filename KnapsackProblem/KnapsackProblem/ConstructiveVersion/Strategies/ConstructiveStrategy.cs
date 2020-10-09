using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public abstract class ConstructiveStrategy: AbstractStrategy<ConstructiveResult, KnapsackInstance>
    {
        protected KnapsackConfiguration BestConfiguration;
    }
}

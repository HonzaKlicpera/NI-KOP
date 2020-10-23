using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public class ConstructiveDPBoth : ConstructiveStrategy
    {
        public override void FreeAll()
        {
        }

        public override ConstructiveResult Solve(KnapsackInstance instance)
        {
            ConstructiveStrategy strategy;
            if (instance.KnapsackSize < instance.GetPriceOfAllItems())
                strategy = new ConstructiveDPCapacity();
            else
                strategy = new ConstructiveDPPrice();

            return strategy.Solve(instance);
        }
    }
}

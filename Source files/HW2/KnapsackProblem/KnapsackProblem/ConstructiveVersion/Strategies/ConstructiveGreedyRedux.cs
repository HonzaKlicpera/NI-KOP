using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public class ConstructiveGreedyRedux : ConstructiveStrategy
    {
        public override void FreeAll()
        {
            
        }

        public override ConstructiveResult Solve(KnapsackInstance instance)
        {
            var greedyStrategy = new ConstructiveGreedy();
            var greedySolution = greedyStrategy.Solve(instance);

            var sorted = new List<KnapsackItem>(instance.Items);
            sorted.OrderByDescending(i => i.Price);
            var addedItemsVector = new bool[instance.ItemCount];
            var currentPrice = 0;
            var currentWeight = 0;

            foreach (var item in sorted)
            {
                if(item.Weight <= instance.KnapsackSize)
                {
                    currentPrice = item.Price;
                    currentWeight = item.Weight;
                    addedItemsVector[item.Id] = true;
                }
            }

            if (currentPrice <= greedySolution.Configuration.Price)
                return greedySolution;
            return new ConstructiveResult
            {
                KnapsackInstance = instance,
                Configuration = new KnapsackConfiguration
                {
                    ItemVector = addedItemsVector.ToList(),
                    Price = currentPrice,
                    Weight = currentWeight
                }
            };
        }
    }
}

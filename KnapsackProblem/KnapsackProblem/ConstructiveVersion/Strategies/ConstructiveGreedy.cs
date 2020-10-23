using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public class ConstructiveGreedy : ConstructiveStrategy
    {
        public override void FreeAll()
        {
            
        }

        public override ConstructiveResult Solve(KnapsackInstance instance)
        {
            //Make a shallow copy of instances list in order to not affect next possible execution
            var sorted = new List<KnapsackItem>(instance.Items);
            sorted.OrderByDescending(i => i.Price / i.Weight);
            var addedItemsVector = new bool[instance.ItemCount];

            var currentWeight = 0;
            var currentPrice = 0;
            foreach(var item in sorted)
            {
                if (currentWeight + item.Weight > instance.KnapsackSize)
                    break;
                currentPrice += item.Price;
                currentWeight += item.Weight;
                addedItemsVector[item.Id] = true;
            }

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

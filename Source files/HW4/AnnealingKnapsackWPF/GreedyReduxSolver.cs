using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackAnnealing
{
    public class GreedyReduxSolver
    {
        public KnapsackResult Solve(KnapsackInstance instance)
        {
            var sorted = instance.Items.OrderByDescending(i => i.Price / i.Weight);
            var greedyResult = GreedyResult(instance, sorted);

            var biggestLegalItem = sorted.FirstOrDefault(i => i.Weight <= instance.KnapsackSize);
            var addedItemsVector = new bool[instance.ItemCount];
                
            if (biggestLegalItem == null || biggestLegalItem.Price <= greedyResult.Configuration.Price)
                return greedyResult;

            addedItemsVector[biggestLegalItem.Id] = true;
            return new KnapsackResult
            {
                KnapsackInstance = instance,
                Configuration = new KnapsackConfiguration
                {
                    ItemVector = addedItemsVector.ToList(),
                    Price = biggestLegalItem.Price,
                    Weight = biggestLegalItem.Weight
                }
            };
        }


        KnapsackResult GreedyResult(KnapsackInstance instance, IOrderedEnumerable<KnapsackItem> sortedItems)
        {
            var addedItemsVector = new bool[instance.ItemCount];
            var currentWeight = 0;
            var currentPrice = 0;

            foreach (var item in sortedItems)
            {
                if (currentWeight + item.Weight > instance.KnapsackSize)
                    break;
                currentPrice += item.Price;
                currentWeight += item.Weight;
                addedItemsVector[item.Id] = true;
            }

            return new KnapsackResult
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

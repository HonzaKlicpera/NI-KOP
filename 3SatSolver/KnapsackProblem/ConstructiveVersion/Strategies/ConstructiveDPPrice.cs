using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public class ConstructiveDPPrice: ConstructiveDP
    {
        public override ConstructiveResult Solve(KnapsackInstance instance)
        {
            if (instance.ItemCount == 0)
                return new ConstructiveResult
                {
                    KnapsackInstance = instance,
                    Configuration = new KnapsackConfiguration { ItemVector = new List<bool>(), Price = 0, Weight = 0}
                };
            FillTable(instance);
            int bestPrice;
            var bestCell = FindBestCell(instance, out bestPrice);
            var itemVector = GetItemVector(bestCell);

            var knapsackConfiguration = new KnapsackConfiguration
            {
                ItemVector = itemVector,
                Price = bestPrice
            };

            return new ConstructiveResult
            {
                KnapsackInstance = instance,
                Configuration = knapsackConfiguration
            };
        }

        private DPCell FindBestCell(KnapsackInstance instance, out int bestPrice)
        {
            bestPrice = toVisit.Dequeue();
            foreach (var priceIndex in toVisit)
            {
                if (priceIndex > bestPrice)
                    bestPrice = priceIndex;
            }
            return memoryTable[bestPrice, instance.ItemCount-1];
        }

        protected override void InitializeTable(KnapsackInstance instance)
        {
            memoryTable = new DPCell[instance.GetPriceOfAllItems() + 1, instance.ItemCount];
            toVisit = new Queue<int>();

            toVisit.Enqueue(0);
            memoryTable[0, 0] = new DPCell { AddedItem = false, Value = 0 };
            var firstItem = instance.Items.First();
            if (firstItem.Weight <= instance.KnapsackSize && firstItem.Price > 0)
            {
                toVisit.Enqueue(firstItem.Price);
                memoryTable[firstItem.Price, 0] = new DPCell { AddedItem = true, Value = firstItem.Weight };
            }
        }

        protected override void TryAddCell(int currentIndexValue, int currentItemIndex, int maxWeight, Queue<int> toVisitNext, KnapsackItem item = null)
        {
            var currentCell = memoryTable[currentIndexValue, currentItemIndex];

            var newWeight = currentCell.Value;
            var newPrice = currentIndexValue;
            if (item != null)
            {
                newPrice += item.Price;
                newWeight += item.Weight;
            }
            //Weight limit reached
            if (newWeight > maxWeight)
                return;

            var nextCell = memoryTable[newPrice, currentItemIndex + 1];
            //Add the new cell into the table if it is better than the one previously placed there
            if (nextCell == null || nextCell.Value > newWeight)
            {
                memoryTable[newPrice, currentItemIndex + 1] = new DPCell
                {
                    AddedItem = (item != null),
                    PreviousCell = currentCell,
                    Value = newWeight
                };
                toVisitNext.Enqueue(newPrice);
            }
        }

    }
}

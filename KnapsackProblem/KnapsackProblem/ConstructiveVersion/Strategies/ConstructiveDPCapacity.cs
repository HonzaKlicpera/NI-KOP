using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{ 

    public class ConstructiveDPCapacity : ConstructiveDP
    {

        public override ConstructiveResult Solve(KnapsackInstance instance)
        {
            FillTable(instance);
            var bestCell = FindBestCell(instance);
            var itemVector = GetItemVector(bestCell);

            var knapsackConfiguration = new KnapsackConfiguration
            {
                ItemVector = itemVector,
                Price = bestCell.Value
            };

            return new ConstructiveResult
            {
                KnapsackInstance = instance,
                Solution = knapsackConfiguration
            };
        }

        private DPCell FindBestCell(KnapsackInstance instance)
        {
            DPCell bestCell = memoryTable[toVisit.Dequeue(), instance.ItemCount-1];
            foreach (var weightIndex in toVisit)
            {
                var currentCell = memoryTable[weightIndex, instance.ItemCount-1];
                if (currentCell.Value > bestCell.Value)
                    bestCell = currentCell;
            }
            return bestCell;
        }

        protected override void InitializeTable(KnapsackInstance instance)
        {
            memoryTable = new DPCell[instance.KnapsackSize+1, instance.ItemCount];
            toVisit = new Queue<int>();

            toVisit.Enqueue(0);
            memoryTable[0, 0] = new DPCell { AddedItem = false, Value = 0 };
            var firstItem = instance.Items.First();
            if (firstItem.Weight <= instance.KnapsackSize)
            {
                toVisit.Enqueue(firstItem.Weight);
                memoryTable[firstItem.Weight, 0] = new DPCell { AddedItem = true, Value = firstItem.Price };
            }
        }

        protected override void TryAddCell(int currentIndexValue, int currentItemIndex, int maxWeight, Queue<int> toVisitNext, KnapsackItem item = null)
        {
            var currentCell = memoryTable[currentIndexValue, currentItemIndex];

            var newPrice = currentCell.Value;
            var newWeight = currentIndexValue;
            if(item != null)
            {
                newPrice += item.Price;
                newWeight += item.Weight;
            }
            //Weight limit reached
            if (newWeight > maxWeight)
                return;

            var nextCell = memoryTable[newWeight, currentItemIndex + 1];
            //Add the new cell into the table if it is better than the one previously placed there
            if (nextCell == null || nextCell.Value < newPrice) {
                memoryTable[newWeight, currentItemIndex + 1] = new DPCell
                {
                    AddedItem = (item == null),
                    PreviousCell = currentCell,
                    Value = newPrice
                };
                toVisitNext.Enqueue(newWeight);
            }
        }
    }
}

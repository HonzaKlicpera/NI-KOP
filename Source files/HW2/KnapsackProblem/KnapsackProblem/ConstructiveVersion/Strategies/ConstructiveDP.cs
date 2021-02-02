using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public abstract class ConstructiveDP: ConstructiveStrategy
    {
        protected class DPCell
        {
            //The value specific for the DP variant (Weight or Price)
            public int Value { get; set; }
            public DPCell PreviousCell { get; set; }
            public bool AddedItem { get; set; }
        }

        protected DPCell[,] memoryTable;
        protected Queue<int> toVisit;

        protected abstract void InitializeTable(KnapsackInstance instance);

        public override void FreeAll()
        {
            memoryTable = null;
            toVisit = null;
        }


        protected void FillTable(KnapsackInstance instance)
        {
            InitializeTable(instance);

            for (int itemIndex = 0; itemIndex < instance.ItemCount - 1; itemIndex++)
            {
                var toVisitNext = new Queue<int>();
                //Only visit the cells that have been added in the previous go
                foreach (var weight in toVisit)
                {
                    //do not add item
                    TryAddCell(weight, itemIndex, instance.KnapsackSize, toVisitNext);
                    //add item
                    TryAddCell(weight, itemIndex, instance.KnapsackSize, toVisitNext, instance.Items[itemIndex + 1]);
                }
                toVisit = toVisitNext;
            }
        }

        protected abstract void TryAddCell(int currentIndexValue, int currentItemIndex, int maxWeight, Queue<int> toVisitNext, KnapsackItem item = null);


        protected List<bool> GetItemVector(DPCell lastCell)
        {
            var itemsVector = new List<bool>();
            var currentCell = lastCell;
            while (currentCell != null)
            {
                itemsVector.Add(currentCell.AddedItem);
                currentCell = currentCell.PreviousCell;
            }

            itemsVector.Reverse();
            return itemsVector;
        }

    }
}

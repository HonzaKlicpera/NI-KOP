using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public class ConstructiveFPTAS : ConstructiveStrategy
    {
        public override void FreeAll()
        {
        }

        private ConstructiveResult EmptyResult(KnapsackInstance instance)
        {
            return new ConstructiveResult
            {
                KnapsackInstance = instance,
                Configuration = new KnapsackConfiguration
                {
                    ItemVector = instance.Items.Select(i => false).ToList(),
                    Price = 0,
                    Weight = 0
                },
                Strategy = $"fptas (epsilon:{instance.ApproximationAccuracy})"
            };
        }

        public override ConstructiveResult Solve(KnapsackInstance instance)
        {
            //Select only the items that can fit into the backcpack and make a copy of them
            var modifiedItems = instance.Items
                .Where(i => i.Weight <= instance.KnapsackSize)
                .Select(i => new KnapsackItem { Price = i.Price, Weight = i.Weight })
                .ToList();

            if (modifiedItems.Count() == 0)
                return EmptyResult(instance);

            //Calculate the divider based on the desired accuracy and input instance
            double divider = instance.ApproximationAccuracy * (double)modifiedItems.Max(i => i.Price) / modifiedItems.Count();
            //Convert the price of each item
            foreach (var item in modifiedItems)
            {
                 item.Price = (int) Math.Floor(item.Price / divider);
            }

            //Create a new instance for the DP solver
            var newKnapsackInstance = new KnapsackInstance
            {
                ApproximationAccuracy = instance.ApproximationAccuracy,
                Id = instance.Id,
                Items = modifiedItems,
                KnapsackSize = instance.KnapsackSize
            };

            var dpPriceStrategy = new ConstructiveDPPrice();
            var solution = dpPriceStrategy.Solve(newKnapsackInstance);

            //Include the removed items in the result item vector
            solution.Configuration.ItemVector = ConvertItemVector(solution.Configuration.ItemVector, instance);
            solution.KnapsackInstance = instance;

            //Calculate the total price using the solution item vector and the original item prices
            var solutionPrice = instance.Items.Aggregate(0, 
                (acc, item) => acc + item.Price * Convert.ToInt32(solution.Configuration.ItemVector[item.Id]));
            solution.Configuration.Price = solutionPrice;
            solution.Strategy = $"fptas (epsilon:{instance.ApproximationAccuracy})";

            return solution;
        }

        /// <summary>
        /// Because some items might be removed from the knapsack before reaching the DP algorithm 
        /// (due to being single-handedly too big for the knapsack), the item vector needs to be expanded
        /// to include these removed items (they will always be a 0 in the vector)
        /// </summary>
        /// <param name="result"></param>
        /// <param name="originalInstance"></param>
        /// <returns></returns>
        List<bool> ConvertItemVector(IList<bool> itemVector, KnapsackInstance originalInstance)
        {
            //No items were removed
            if (itemVector.Count == originalInstance.ItemCount)
                return itemVector.ToList();

            var updatedSolutionVector = new List<bool>();
            var solutionVectorIndex = 0;
            //Go through every item in the original instance
            foreach (var item in originalInstance.Items)
            {
                //This item was removed, add it into the item vector
                if (item.Weight > originalInstance.KnapsackSize)
                    updatedSolutionVector.Add(false);
                //This item was not removed, just copy the result
                else
                {
                    updatedSolutionVector.Add(itemVector[solutionVectorIndex]);
                    solutionVectorIndex++;
                }
            }
            return updatedSolutionVector;
        }
    }
}

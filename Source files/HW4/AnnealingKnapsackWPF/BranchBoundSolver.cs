using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public class BranchBoundSolver
    {
        private KnapsackConfiguration BestConfiguration;

        public KnapsackResult Solve(KnapsackInstance instance)
        {
            BestConfiguration = new KnapsackConfiguration { Price = 0, Weight = 0, ItemVector = new bool[instance.ItemCount].ToList() };

            FindBestConfiguration(0, new KnapsackConfiguration { Price = 0, Weight = 0, ItemVector = new List<bool>() }, instance.GetPriceOfAllItems(), instance);

            var result = new KnapsackResult
            {
                KnapsackInstance = instance,
                Configuration = BestConfiguration
            };
            return result;
        }

        private void FindBestConfiguration(int itemIndex, KnapsackConfiguration currentConfiguration, int remainingItemsPrice, KnapsackInstance instance)
        {

            //Check for a leaf node
            if (itemIndex > instance.ItemCount - 1)
            {
                if (currentConfiguration.Weight <= instance.KnapsackSize && currentConfiguration.Price > BestConfiguration.Price)
                    BestConfiguration = currentConfiguration;
                return;
            }
            //Check for price bound
            if (currentConfiguration.Price + remainingItemsPrice <= BestConfiguration.Price)
                return;
            //Check for weight overload
            if (currentConfiguration.Weight > instance.KnapsackSize)
                return;

            var currentItem = instance.Items[itemIndex];

            var leftConfiguration = new KnapsackConfiguration
            {
                Price = currentConfiguration.Price,
                Weight = currentConfiguration.Weight,
                ItemVector = new List<bool>(currentConfiguration.ItemVector)
            };
            leftConfiguration.ItemVector.Add(false);

            var rightConfiguration = new KnapsackConfiguration
            {
                Price = currentConfiguration.Price + currentItem.Price,
                Weight = currentConfiguration.Weight + currentItem.Weight,
                ItemVector = new List<bool>(currentConfiguration.ItemVector)
            };
            rightConfiguration.ItemVector.Add(true);

            FindBestConfiguration(itemIndex + 1, leftConfiguration, remainingItemsPrice - currentItem.Price, instance);
            FindBestConfiguration(itemIndex + 1, rightConfiguration, remainingItemsPrice - currentItem.Price, instance);
        }
    }
}

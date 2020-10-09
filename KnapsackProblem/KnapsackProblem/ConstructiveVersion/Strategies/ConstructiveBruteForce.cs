using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion.Strategies
{
    public struct KnapsackConfiguration
    {
        public int Price { get; set; }
        public int Weight { get; set; }

        public IList<bool> SolutionVector { get; set; }
    }

    public class ConstructiveBruteForce : ConstructiveStrategy
    {
        public override ConstructiveResult Solve(KnapsackInstance instance)
        {
            BestConfiguration = new KnapsackConfiguration { Price = int.MinValue, Weight = 0, SolutionVector = new List<bool>()};
            numberOfSteps = 0;

            FindBestConfiguration(0, new KnapsackConfiguration { Price = 0, Weight = 0, SolutionVector = new List<bool>() }, instance);

            var result = new ConstructiveResult
            {
                KnapsackInstance = instance,
                NumberOfSteps = numberOfSteps,
                Solution = new KnapsackSolution { Price = BestConfiguration.Price, SolutionVector = BestConfiguration.SolutionVector }
            };
            return result;
        }

        private void FindBestConfiguration(int itemIndex, KnapsackConfiguration currentConfiguration, KnapsackInstance instance)
        {
            numberOfSteps++;
            if (itemIndex > instance.ItemCount - 1)
            {
                if (currentConfiguration.Weight <= instance.KnapsackSize && currentConfiguration.Price > BestConfiguration.Price)
                    BestConfiguration = currentConfiguration;
                return;
            }
            var currentItem = instance.Items[itemIndex];

            var leftConfiguration = new KnapsackConfiguration
            {
                Price = currentConfiguration.Price,
                Weight = currentConfiguration.Weight,
                SolutionVector = new List<bool>(currentConfiguration.SolutionVector)
            };
            leftConfiguration.SolutionVector.Add(false);

            var rightConfiguration = new KnapsackConfiguration
            {
                Price = currentConfiguration.Price + currentItem.Price,
                Weight = currentConfiguration.Weight + currentItem.Weight, 
                SolutionVector = new List<bool>(currentConfiguration.SolutionVector)
            };
            rightConfiguration.SolutionVector.Add(true);

            FindBestConfiguration(itemIndex + 1, leftConfiguration, instance);
            FindBestConfiguration(itemIndex + 1, rightConfiguration, instance);
        }
    }
}

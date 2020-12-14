using KnapsackAnnealing.Solver;
using KnapsackAnnealing.Solver.StartingPositionStrategies;
using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingKnapsackWPF.Solver.StartingPositionStrategies
{
    public class RandomStartingPos : IStartingPositionStrategy
    {
        private Random random;

        public RandomStartingPos(int seed)
        {
            random = new Random(seed);
        }

        public KnapsackConfiguration GetStartingPosition(SimulatedAnnealingSolver solverInstance)
        {
            var configuration = new KnapsackConfiguration { Price = 0, Weight = 0};
            var itemVector = new List<bool>();
            foreach(var item in solverInstance.Instance.Items)
            {
                var added = Convert.ToBoolean(random.Next(0, 1));
                itemVector.Add(added);
                if (added)
                {
                    configuration.Price += item.Price;
                    configuration.Weight += item.Weight;
                }
            }
            configuration.ItemVector = itemVector;

            return configuration;
        }
    }
}

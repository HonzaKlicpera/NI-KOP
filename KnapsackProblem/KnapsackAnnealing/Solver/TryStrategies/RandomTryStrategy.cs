using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.TryStrategies
{
    public class RandomTryStrategy : ITryStrategy
    {
        Random random;

        public RandomTryStrategy(int seed)
        {
            random = new Random(seed);
        }
        //Performs a random bit-flip
        public bool Try(SimulatedAnnealingSolver solverInstance, ref KnapsackConfiguration currentConfiguration)
        {
            var bitToFlip = random.Next(0, solverInstance.Instance.ItemCount - 1);
            var triedConfiguration = new KnapsackConfiguration(currentConfiguration);

            triedConfiguration.ItemVector[bitToFlip] = !triedConfiguration.ItemVector[bitToFlip];
            if (triedConfiguration.ItemVector[bitToFlip])
            {
                triedConfiguration.Price += solverInstance.Instance.Items[bitToFlip].Price;
                triedConfiguration.Weight += solverInstance.Instance.Items[bitToFlip].Weight;
            }
            else
            {
                triedConfiguration.Price -= solverInstance.Instance.Items[bitToFlip].Price;
                triedConfiguration.Weight -= solverInstance.Instance.Items[bitToFlip].Weight;
            }
            triedConfiguration.Score = solverInstance.Options.ScoreStrategy.Score(triedConfiguration, solverInstance);

            if (triedConfiguration.Score >= currentConfiguration.Score)
            {
                currentConfiguration = triedConfiguration;
                return true;
            }

            var delta = triedConfiguration.Score - currentConfiguration.Score;
            if (random.NextDouble() < Math.Exp(delta / solverInstance.CurrentTemperature))
            {
                currentConfiguration = triedConfiguration;
                return true;
            }
            return false;
        }
    }
}

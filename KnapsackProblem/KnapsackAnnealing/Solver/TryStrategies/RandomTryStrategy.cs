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
        public KnapsackConfiguration Try(SimulatedAnnealingSolver solverInstance)
        {
            var bitToFlip = random.Next(0, solverInstance.Instance.ItemCount - 1);
            var configuration = new KnapsackConfiguration(solverInstance.CurrentConfiguration);

            configuration.ItemVector[bitToFlip] = !configuration.ItemVector[bitToFlip];
            if (configuration.ItemVector[bitToFlip])
            {
                configuration.Price += solverInstance.Instance.Items[bitToFlip].Price;
                configuration.Weight += solverInstance.Instance.Items[bitToFlip].Weight;
            }
            else
            {
                configuration.Price -= solverInstance.Instance.Items[bitToFlip].Price;
                configuration.Weight -= solverInstance.Instance.Items[bitToFlip].Weight;
            }
            configuration.Score = solverInstance.Options.ScoreStrategy.Score(configuration, solverInstance);

            if (configuration.Score >= solverInstance.CurrentConfiguration.Score)
                return configuration;

            var delta = configuration.Score - solverInstance.CurrentConfiguration.Score;
            if (random.NextDouble() < Math.Exp(delta / solverInstance.CurrentTemperature))
                return configuration;
            return solverInstance.CurrentConfiguration;
        }
    }
}

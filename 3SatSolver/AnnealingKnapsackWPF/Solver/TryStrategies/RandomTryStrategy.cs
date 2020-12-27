using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.TryStrategies
{
    public class RandomTryStrategy : ITryStrategy
    {
        Random random;

        public RandomTryStrategy(int seed)
        {
            random = new Random(seed);
        }
        //Performs a random bit-flip
        public bool Try(SimulatedAnnealingSolver solverInstance, ref SatConfiguration currentConfiguration)
        {
            var bitToFlip = random.Next(0, solverInstance.Instance.Literals.Count - 1);
            var triedConfiguration = new SatConfiguration(currentConfiguration);

            triedConfiguration.Valuations[bitToFlip] = !triedConfiguration.Valuations[bitToFlip];
            
            triedConfiguration.Score = solverInstance.Options.ScoreStrategy.CalculateScore(triedConfiguration, solverInstance);

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

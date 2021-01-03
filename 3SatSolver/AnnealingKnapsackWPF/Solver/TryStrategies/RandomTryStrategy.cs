using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.TryStrategies
{
    public class RandomTryStrategy : TryStrategy
    {
        public RandomTryStrategy(int seed) : base(seed)
        {
        }
        //Performs a random bit-flip
        public override bool Try(SimulatedAnnealingSolver solverInstance, ref SatConfiguration currentConfiguration)
        {
            var bitToFlip = random.Next(0, solverInstance.SatInstance.Literals.Count - 1);
            var triedConfiguration = new SatConfiguration(currentConfiguration);

            triedConfiguration.Valuations[bitToFlip] = !triedConfiguration.Valuations[bitToFlip];
            
            triedConfiguration.Score = solverInstance.Options.ScoreStrategy.CalculateScore(triedConfiguration, solverInstance);

            if(Accept(triedConfiguration, currentConfiguration, solverInstance))
            {
                currentConfiguration = triedConfiguration;
                return true;
            }
            
            return false;
        }
    }
}

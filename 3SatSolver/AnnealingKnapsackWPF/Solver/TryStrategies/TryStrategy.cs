using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.TryStrategies
{
    public abstract class TryStrategy
    {
        protected Random random;

        public TryStrategy(int seed)
        {
            random = new Random(seed);
        }

        public abstract bool Try(SimulatedAnnealingSolver solverInstance, ref SatConfiguration currentConfiguration);

        protected bool Accept(SatConfiguration triedConfiguration, SatConfiguration currentConfiguration, SimulatedAnnealingSolver solverInstance)
        {
            //Tried configuration is better
            if (triedConfiguration.Score >= currentConfiguration.Score)
                return true;

            var delta = triedConfiguration.Score - currentConfiguration.Score;
            var scaledTemperature = solverInstance.CurrentTemperature * currentConfiguration.Instance.GetSumOfWeights();
            //The configuration is not better, but will be accepted under the given probability
            if (random.NextDouble() < Math.Exp(delta / scaledTemperature))
                return true;
            return false;
        }
    }
}

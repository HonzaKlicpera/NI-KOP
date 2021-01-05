using AnnealingWPF.Solver;
using AnnealingWPF.Solver.StartingPositionStrategies;
using AnnealingWPF.Common;
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

        public SatConfiguration GetStartingPosition(SimulatedAnnealingSolver solverInstance)
        {
            return SatConfiguration.RandomConfiguration(solverInstance.SatInstance, random);
        }
    }
}

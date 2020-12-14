using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.TryStrategies
{
    public interface ITryStrategy
    {
        public bool Try(SimulatedAnnealingSolver solverInstance, ref KnapsackConfiguration knapsackConfiguration);
    }
}

using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.TryStrategies
{
    public interface ITryStrategy
    {
        public bool Try(SimulatedAnnealingSolver solverInstance, ref SatConfiguration knapsackConfiguration);
    }
}

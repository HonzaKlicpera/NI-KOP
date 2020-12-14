using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.FrozenStrategies
{
    public interface IFrozenStrategy
    {
        public bool Frozen(SimulatedAnnealingSolver solverInstance);
    }
}

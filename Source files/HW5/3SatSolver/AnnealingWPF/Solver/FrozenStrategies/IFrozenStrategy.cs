using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.FrozenStrategies
{
    public interface IFrozenStrategy
    {
        public bool Frozen(SimulatedAnnealingSolver solverInstance);
    }
}

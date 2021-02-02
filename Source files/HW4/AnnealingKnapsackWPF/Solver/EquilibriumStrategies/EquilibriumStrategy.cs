using KnapsackAnnealing.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.EquilibriumStrategies
{
    public abstract class EquilibriumStrategy
    {
        protected int loopCount;
        public EquilibriumStrategy()
        {
            loopCount = 0;
        }

        public abstract bool Equilibrium(SimulatedAnnealingSolver solverInstance);
    }
}

using KnapsackAnnealing.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.EquilibriumStrategies
{
    public interface IEquilibriumStrategy
    {
        public bool Equilibrium(SimulatedAnnealingSolver solverInstance);
    }
}

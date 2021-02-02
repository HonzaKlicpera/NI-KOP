using KnapsackAnnealing.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.CoolingStrategies
{
    public interface ICoolingStrategy
    {
        public float Cool(SimulatedAnnealingSolver solverInstance);
    }
}

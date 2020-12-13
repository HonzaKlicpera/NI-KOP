using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.FrozenStrategies
{
    public class ConstantFrozenStrategy : IFrozenStrategy
    {
        public bool Frozen(SimulatedAnnealingSolver solverInstance)
        {
            if (solverInstance.CurrentTemperature <= AnnealingSolverConfig.MIN_CONST_TEMPERATURE)
                return true;
            return false;
        }
    }
}

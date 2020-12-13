using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.FrozenStrategies
{
    public class MoveBasedFrozenStrategy : IFrozenStrategy
    {
        public bool Frozen(SimulatedAnnealingSolver solverInstance)
        {
            if (solverInstance.CurrentTemperature <= AnnealingSolverConfig.MIN_CONST_TEMPERATURE
                || solverInstance.UnacceptedInARow >= AnnealingSolverConfig.MAX_UNSUCCESSFUL_TRIES)
                return true;
            return false;
        }
    }
}

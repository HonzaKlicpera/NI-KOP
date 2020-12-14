using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.FrozenStrategies
{
    public class MoveBasedFrozenStrategy : IFrozenStrategy
    {
        public bool Frozen(SimulatedAnnealingSolver solverInstance)
        {
            if (solverInstance.CurrentTemperature <= solverInstance.Options.MinimalTemperature
                || solverInstance.UnacceptedInARow >= solverInstance.Options.MaxUnaccepted)
                return true;
            return false;
        }
    }
}

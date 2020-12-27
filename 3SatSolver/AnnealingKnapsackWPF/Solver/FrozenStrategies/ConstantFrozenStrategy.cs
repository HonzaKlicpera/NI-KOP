using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.FrozenStrategies
{
    public class ConstantFrozenStrategy : IFrozenStrategy
    {
        public bool Frozen(SimulatedAnnealingSolver solverInstance)
        {
            if (solverInstance.CurrentTemperature <= solverInstance.Options.MinimalTemperature)
                return true;
            return false;
        }
    }
}

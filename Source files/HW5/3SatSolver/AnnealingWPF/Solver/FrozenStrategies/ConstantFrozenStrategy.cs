using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.FrozenStrategies
{
    public class ConstantFrozenStrategy : IFrozenStrategy
    {
        public bool Frozen(SimulatedAnnealingSolver solverInstance)
        {
            if (solverInstance.CurrentTemperature <= solverInstance.ScaledMinTemperature)
                return true;
            return false;
        }
    }
}

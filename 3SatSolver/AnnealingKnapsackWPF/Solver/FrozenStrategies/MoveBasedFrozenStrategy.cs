using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.FrozenStrategies
{
    public class MoveBasedFrozenStrategy : IFrozenStrategy
    {
        public bool Frozen(SimulatedAnnealingSolver solverInstance)
        {
            float rejectedRatio;
            if (solverInstance.EquilibriumSteps == 0)
                rejectedRatio = 0.0f;
            else
                rejectedRatio = 1.0f - ((float)solverInstance.AcceptedDuringEquilibrium / solverInstance.EquilibriumSteps);

            if (solverInstance.CurrentTemperature <= solverInstance.ScaledMinTemperature
                || rejectedRatio >= solverInstance.Options.MaxRejectedRatio)
                return true;
            return false;
        }
    }
}

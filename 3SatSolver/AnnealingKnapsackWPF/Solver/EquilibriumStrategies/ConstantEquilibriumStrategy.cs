using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.EquilibriumStrategies
{
    public class ConstantEquilibriumStrategy : EquilibriumStrategy
    {
        public override bool Equilibrium(SimulatedAnnealingSolver solverInstance)
        {
            if (loopCount >= solverInstance.Options.BaseEquilibriumSteps * solverInstance.Instance.Literals.Count)
            {
                loopCount = 0;
                return false;
            }
            loopCount++;
            return true;
        }
    }
}

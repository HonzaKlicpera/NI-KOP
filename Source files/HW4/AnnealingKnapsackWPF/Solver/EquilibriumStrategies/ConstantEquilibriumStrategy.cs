using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.EquilibriumStrategies
{
    public class ConstantEquilibriumStrategy : EquilibriumStrategy
    {
        public override bool Equilibrium(SimulatedAnnealingSolver solverInstance)
        {
            if (loopCount >= solverInstance.Options.BaseEquilibriumSteps * solverInstance.Instance.ItemCount)
            {
                loopCount = 0;
                return false;
            }
            loopCount++;
            return true;
        }
    }
}

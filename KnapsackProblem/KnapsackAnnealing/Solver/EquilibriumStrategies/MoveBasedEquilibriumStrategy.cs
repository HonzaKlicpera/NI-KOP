using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.EquilibriumStrategies
{
    public class MoveBasedEquilibriumStrategy : EquilibriumStrategy
    {
        public override bool Equilibrium(SimulatedAnnealingSolver solverInstance)
        {
            if (loopCount >= AnnealingSolverConfig.CONST_EQUILIBRIUM_LOOP_COUNT * solverInstance.Instance.ItemCount)
            {
                loopCount = 0;
                return false;
            }
            loopCount++;
            return true;
        }
    }
}

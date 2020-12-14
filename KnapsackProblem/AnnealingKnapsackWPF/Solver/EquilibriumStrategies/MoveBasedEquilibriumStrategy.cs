using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.EquilibriumStrategies
{
    public class MoveBasedEquilibriumStrategy : EquilibriumStrategy
    {
        public override bool Equilibrium(SimulatedAnnealingSolver solverInstance)
        {
            var N = solverInstance.Options.BaseEquilibriumSteps * solverInstance.Instance.ItemCount;
            if (loopCount >= N * 2 || solverInstance.AcceptedDuringEquilibrium >= N)
            {
                loopCount = 0;
                return false;
            }
            loopCount++;
            return true;
        }
    }
}

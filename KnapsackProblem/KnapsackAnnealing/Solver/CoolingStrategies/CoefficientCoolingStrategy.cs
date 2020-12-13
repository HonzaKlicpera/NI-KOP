using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.CoolingStrategies
{
    public class CoefficientCoolingStrategy : ICoolingStrategy
    {
        public float Cool(SimulatedAnnealingSolver solverInstance)
        {
            return solverInstance.CurrentTemperature * AnnealingSolverConfig.COOLING_COEFFICIENT;
        }
    }
}

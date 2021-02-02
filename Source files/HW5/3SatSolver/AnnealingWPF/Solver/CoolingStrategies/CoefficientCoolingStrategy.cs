using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.CoolingStrategies
{
    public class CoefficientCoolingStrategy : ICoolingStrategy
    {
        public float Cool(SimulatedAnnealingSolver solverInstance)
        {
            return solverInstance.CurrentTemperature * solverInstance.Options.CoolingCoefficient;
        }
    }
}

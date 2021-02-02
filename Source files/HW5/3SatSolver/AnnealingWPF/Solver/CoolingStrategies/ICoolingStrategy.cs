using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.CoolingStrategies
{
    public interface ICoolingStrategy
    {
        public float Cool(SimulatedAnnealingSolver solverInstance);
    }
}

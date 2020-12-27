using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.StartingPositionStrategies
{
    public interface IStartingPositionStrategy
    {
        public SatConfiguration GetStartingPosition(SimulatedAnnealingSolver solverInstance);
    }
}

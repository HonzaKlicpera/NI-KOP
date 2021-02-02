using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.StartingPositionStrategies
{
    public interface IStartingPositionStrategy
    {
        public KnapsackConfiguration GetStartingPosition(SimulatedAnnealingSolver solverInstance);
    }
}

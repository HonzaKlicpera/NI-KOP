using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.StartingPositionStrategies
{
    public class GreedyStartingPos : IStartingPositionStrategy
    {
        public KnapsackConfiguration GetStartingPosition(SimulatedAnnealingSolver solverInstance)
        {
            var greedyResult = new GreedyReduxSolver().Solve(solverInstance.Instance);
            return greedyResult.Configuration;
        }
    }
}

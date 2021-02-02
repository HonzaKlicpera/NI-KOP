using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.ScoreStrategies
{
    public interface IScoreStrategy
    {
        public int Cost(KnapsackConfiguration configuration, SimulatedAnnealingSolver solver);
    }
}

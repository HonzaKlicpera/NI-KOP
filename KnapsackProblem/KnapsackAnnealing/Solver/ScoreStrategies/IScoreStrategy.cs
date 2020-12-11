using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.ScoreStrategies
{
    public interface IScoreStrategy
    {
        public int Score(KnapsackConfiguration configuration, SimulatedAnnealingSolver solver);
    }
}

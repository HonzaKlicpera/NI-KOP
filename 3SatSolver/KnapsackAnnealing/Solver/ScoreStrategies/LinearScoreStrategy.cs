using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.ScoreStrategies
{
    public class LinearScoreStrategy : IScoreStrategy
    {
        public int Score(KnapsackConfiguration configuration, SimulatedAnnealingSolver solver)
        {
            int score = configuration.Price;
            if(configuration.Weight > solver.Instance.KnapsackSize)
            {
                var weightOverrun = configuration.Weight - solver.Instance.KnapsackSize;
                score -= weightOverrun * AnnealingSolverConfig.SCORE_PENALTY_MULTIPLIER;
            }
            return score;
        }
    }
}

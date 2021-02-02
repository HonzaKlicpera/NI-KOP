using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver.ScoreStrategies
{
    public class LinearScoreStrategy : IScoreStrategy
    {
        public int Cost(KnapsackConfiguration configuration, SimulatedAnnealingSolver solver)
        {
            int score = configuration.Price;
            if(configuration.Weight > solver.Instance.KnapsackSize)
            {
                var weightOverrun = configuration.Weight - solver.Instance.KnapsackSize;
                score -= (int) Math.Round(weightOverrun * solver.Options.PenaltyMultiplier);
            }
            return score;
        }
    }
}

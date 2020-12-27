using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.ScoreStrategies
{
    public class LinearScoreStrategy : IScoreStrategy
    {
        public float CalculateScore(SatConfiguration configuration, SimulatedAnnealingSolver solver)
        {
            //Base score is the optimalization value
            float score = configuration.GetOptimalizationValue();
            //Penalize the score if some clauses are unsatisfied
            score -= configuration.NumberOfUnsatisfiedClauses() * solver.Options.PenaltyMultiplier;
            return score;
        }
    }
}

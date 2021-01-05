using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.ScoreStrategies
{
    public class SoftScaledPenaltyScoreStrategy : IScoreStrategy
    {
        public float CalculateScore(SatConfiguration configuration, SimulatedAnnealingSolver solver)
        {
            //Base score is the optimalization value
            float score = configuration.GetOptimalizationValue();

            //Penalize the score based on the number of unsatisfied clauses
            score -= configuration.NumberOfUnsatisfiedClauses() * solver.Options.PenaltyMultiplier * solver.SatInstance.GetSumOfWeights();
            return score;
        }
    }
}

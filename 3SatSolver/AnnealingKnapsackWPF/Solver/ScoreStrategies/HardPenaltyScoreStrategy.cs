using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.ScoreStrategies
{
    public class HardPenaltyScoreStrategy : IScoreStrategy
    {
        public float CalculateScore(SatConfiguration configuration, SimulatedAnnealingSolver solver)
        {
            //Base score is the optimalization value
            float score = configuration.GetOptimalizationValue();

            //Make the score negative if it is unsatisfiable
            if (!configuration.IsSatisfiable())
                score -= configuration.Instance.GetSumOfWeights();

            //Penalize the score further based on the number of unsatisfied clauses
            score -= configuration.NumberOfUnsatisfiedClauses() * solver.Options.PenaltyMultiplier;
            return score;
        }
    }
}

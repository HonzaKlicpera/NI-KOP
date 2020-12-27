using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.ScoreStrategies
{
    public interface IScoreStrategy
    {
        public float CalculateScore(SatConfiguration configuration, SimulatedAnnealingSolver solver);
    }
}

using AnnealingWPF.Solver.CoolingStrategies;
using AnnealingWPF.Solver.EquilibriumStrategies;
using AnnealingWPF.Solver.FrozenStrategies;
using AnnealingWPF.Solver.ScoreStrategies;
using AnnealingWPF.Solver.StartingPositionStrategies;
using AnnealingWPF.Solver.TryStrategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Common
{
    public class AnnealingOptions
    {
        public float BaseStartingTemperature { get; set; }
        public float CoolingCoefficient { get; set; }
        public float MinimalTemperature { get; set; }
        public int BaseEquilibriumSteps { get; set; }
        //Maximal ratio of rejected states during an equilibrium
        public float MaxRejectedRatio { get; set; }
        public float PenaltyMultiplier { get; set; }

        public bool SavePlotInfo { get; set; }

        public ICoolingStrategy CoolStrategy { get; set; }
        public EquilibriumStrategy EquilibriumStrategy { get; set; }
        public IFrozenStrategy FrozenStrategy { get; set; }
        public TryStrategy TryStrategy { get; set; }
        public IStartingPositionStrategy StartingPositionStrategy { get; set; }
        public IScoreStrategy ScoreStrategy { get; set; }
    }
}

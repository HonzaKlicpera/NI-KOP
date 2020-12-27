using KnapsackAnnealing.Solver.CoolingStrategies;
using KnapsackAnnealing.Solver.EquilibriumStrategies;
using KnapsackAnnealing.Solver.FrozenStrategies;
using KnapsackAnnealing.Solver.ScoreStrategies;
using KnapsackAnnealing.Solver.StartingPositionStrategies;
using KnapsackAnnealing.Solver.TryStrategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Common
{
    public class AnnealingOptions
    {
        public float BaseStartingTemperature { get; set; }
        public float CoolingCoefficient { get; set; }
        public float MinimalTemperature { get; set; }

        public ICoolingStrategy CoolStrategy { get; set; }
        public EquilibriumStrategy EquilibriumStrategy { get; set; }
        public IFrozenStrategy FrozenStrategy { get; set; }
        public ITryStrategy TryStrategy { get; set; }
        public IStartingPositionStrategy StartingPositionStrategy { get; set; }
        public IScoreStrategy ScoreStrategy { get; set; }
    }
}

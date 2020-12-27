using AnnealingWPF.Common;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver
{
    public class SimulatedAnnealingSolver
    {
        public SatInstance Instance { get; private set; }
        public AnnealingOptions Options { get; private set; }
        public ulong NumberOfSteps;

        public int AcceptedDuringEquilibrium { get; private set; }
        public int EquilibriumSteps { get; private set; }

        public float CurrentTemperature { get; private set; }
        public SatConfiguration BestConfiguration { get; private set; }
        private SatConfiguration currentConfiguration;


        public SimulatedAnnealingSolver(SatInstance instance, AnnealingOptions options)
        {
            Instance = instance;
            Options = options;
            NumberOfSteps = 0;
        }

        public SatResult Solve()
        {
            var movesHistory = new List<DataPoint>();
            currentConfiguration = Options.StartingPositionStrategy.GetStartingPosition(this);
            BestConfiguration = currentConfiguration;
            CurrentTemperature = Options.BaseStartingTemperature;
            while (!Options.FrozenStrategy.Frozen(this))
            {
                AcceptedDuringEquilibrium = 0;
                EquilibriumSteps = 0;

                while(Options.EquilibriumStrategy.Equilibrium(this))
                {
                    EquilibriumSteps++;
                    movesHistory.Add(new DataPoint((int) NumberOfSteps + EquilibriumSteps, currentConfiguration.GetOptimalizationValue()));
                    //Try to accept a new state
                    if (Options.TryStrategy.Try(this, ref currentConfiguration))
                        AcceptedDuringEquilibrium++;
                    //Check if new maximum has been found
                    if (currentConfiguration.GetOptimalizationValue() > BestConfiguration.GetOptimalizationValue() && currentConfiguration.IsSatisfiable())
                        BestConfiguration = currentConfiguration;
                }
                CurrentTemperature = Options.CoolStrategy.Cool(this);
                NumberOfSteps += (ulong) EquilibriumSteps;
            }
            return new SatResult { Configuration = BestConfiguration,
                SatInstance = Instance,
                NumberOfSteps = NumberOfSteps,
                MovesHistory = movesHistory};
        }
    }
}

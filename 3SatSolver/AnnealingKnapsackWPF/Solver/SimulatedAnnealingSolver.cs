using AnnealingWPF.Common;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver
{
    public class SimulatedAnnealingSolver
    {
        public const int MAX_RESTART_COUNT = 3;
        public const float EQUILIBRIUM_RESTART_MULTIPLIER = 1.2f;

        public SatInstance SatInstance { get; private set; }
        public AnnealingOptions Options { get; private set; }
        public ulong NumberOfSteps;

        public int AcceptedDuringEquilibrium { get; private set; }
        public int EquilibriumSteps { get; private set; }

        public float CurrentTemperature { get; private set; }
        public SatConfiguration BestConfiguration { get; private set; }
        private SatConfiguration currentConfiguration;

        private IList<DataPoint> movesHistory;


        public SimulatedAnnealingSolver(SatInstance instance, AnnealingOptions options)
        {
            SatInstance = instance;
            Options = options;
            NumberOfSteps = 0;
        }

        public SatResult Solve()
        {
            movesHistory = new List<DataPoint>();
            currentConfiguration = Options.StartingPositionStrategy.GetStartingPosition(this);
            BestConfiguration = new SatConfiguration { Instance = SatInstance, Score = 0, Valuations = new List<bool>(new bool[SatInstance.Literals.Count])};
            CurrentTemperature = Options.BaseStartingTemperature;

            var solutionLoopCount = 0;
            while (!RunSolutionLoop() && solutionLoopCount < MAX_RESTART_COUNT - 1)
            {
                solutionLoopCount++;
                CurrentTemperature = Options.BaseStartingTemperature;
                //Options.BaseEquilibriumSteps = (int) (Options.BaseEquilibriumSteps * EQUILIBRIUM_RESTART_MULTIPLIER);
            }

            
            return new SatResult { Configuration = BestConfiguration,
                SatInstance = SatInstance,
                NumberOfSteps = NumberOfSteps,
                MovesHistory = movesHistory,
                NumberOfUnsatisfiedClauses = BestConfiguration.NumberOfUnsatisfiedClauses()
            };
        }

        private bool RunSolutionLoop()
        {
            while (!Options.FrozenStrategy.Frozen(this))
            {
                AcceptedDuringEquilibrium = 0;
                EquilibriumSteps = 0;

                while (Options.EquilibriumStrategy.Equilibrium(this))
                {
                    EquilibriumSteps++;
                    if(Options.SavePlotInfo)
                        movesHistory.Add(new DataPoint((int)NumberOfSteps + EquilibriumSteps, currentConfiguration.GetOptimalizationValue()));
                    //Try to accept a new state
                    if (Options.TryStrategy.Try(this, ref currentConfiguration))
                        AcceptedDuringEquilibrium++;
                    //Check if new maximum has been found
                    if (currentConfiguration.GetOptimalizationValue() > BestConfiguration.GetOptimalizationValue() && currentConfiguration.IsSatisfiable())
                        BestConfiguration = currentConfiguration;
                }
                CurrentTemperature = Options.CoolStrategy.Cool(this);
                NumberOfSteps += (ulong)EquilibriumSteps;
            }

            if (BestConfiguration.IsSatisfiable())
                return true;
            return false;
        }
    }
}

using KnapsackAnnealing.Common;
using KnapsackProblem.Common;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver
{
    public class SimulatedAnnealingSolver
    {
        public KnapsackInstance Instance { get; private set; }
        public AnnealingOptions Options { get; private set; }
        public ulong NumberOfSteps;

        public int UnacceptedInARow { get; private set; }
        public int AcceptedDuringEquilibrium { get; private set; }

        public float CurrentTemperature { get; private set; }
        public KnapsackConfiguration BestConfiguration { get; private set; }
        private KnapsackConfiguration currentConfiguration;


        public SimulatedAnnealingSolver(KnapsackInstance instance, AnnealingOptions options)
        {
            Instance = instance;
            Options = options;
            NumberOfSteps = 0;
        }

        public KnapsackResult Solve()
        {
            var movesHistory = new List<DataPoint>();
            currentConfiguration = Options.StartingPositionStrategy.GetStartingPosition(this);
            BestConfiguration = currentConfiguration;
            CurrentTemperature = Options.BaseStartingTemperature;
            while (!Options.FrozenStrategy.Frozen(this))
            {
                AcceptedDuringEquilibrium = 0;

                while(Options.EquilibriumStrategy.Equilibrium(this))
                {
                    NumberOfSteps++;
                    movesHistory.Add(new DataPoint(NumberOfSteps, currentConfiguration.Price));
                    //Try to accept a new state
                    if (Options.TryStrategy.Try(this, ref currentConfiguration))
                    {
                        UnacceptedInARow = 0;
                        AcceptedDuringEquilibrium++;
                    }
                    else
                        UnacceptedInARow++;
                    //Check if new state is better than the best discovered so far
                    if (currentConfiguration.Price > BestConfiguration.Price && currentConfiguration.Weight <= Instance.KnapsackSize)
                        BestConfiguration = currentConfiguration;
                }
                CurrentTemperature = Options.CoolStrategy.Cool(this);
            }
            return new KnapsackResult { Configuration = BestConfiguration,
                KnapsackInstance = Instance,
                NumberOfSteps = NumberOfSteps,
                MovesHistory = movesHistory};
        }
    }
}

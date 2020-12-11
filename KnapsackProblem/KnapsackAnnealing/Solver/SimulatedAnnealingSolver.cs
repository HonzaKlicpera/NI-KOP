using KnapsackAnnealing.Common;
using KnapsackProblem.Common;
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

        public float CurrentTemperature { get; private set; }
        public KnapsackConfiguration BestConfiguration { get; private set; }
        public KnapsackConfiguration CurrentConfiguration { get; private set; }


        public SimulatedAnnealingSolver(KnapsackInstance instance, AnnealingOptions options)
        {
            Instance = instance;
            Options = options;
            NumberOfSteps = 0;
        }

        public KnapsackResult Solve()
        {
            CurrentConfiguration = Options.StartingPositionStrategy.GetStartingPosition(this);
            CurrentTemperature = Options.StartingTemperature;
            while (!Options.FrozenStrategy.Frozen(this))
            {
                while(!Options.EquilibriumStrategy.Equilibrium(this))
                {
                    NumberOfSteps++;
                    CurrentConfiguration = Options.TryStrategy.Try(this);
                    if (CurrentConfiguration.Price > BestConfiguration.Price && CurrentConfiguration.Weight <= Instance.KnapsackSize)
                        BestConfiguration = CurrentConfiguration;
                }
            }
            return new KnapsackResult { Configuration = BestConfiguration, KnapsackInstance = Instance, NumberOfSteps = NumberOfSteps};
        }
    }
}

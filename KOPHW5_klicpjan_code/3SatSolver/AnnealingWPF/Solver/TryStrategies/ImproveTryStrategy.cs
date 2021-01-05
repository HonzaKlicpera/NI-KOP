using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Solver.TryStrategies
{
    public class ImproveTryStrategy : TryStrategy
    {
        private double randomNeighborProb;
        private double randomNewProb;
        private double improveScoreProb;
        private double improveSatisProb;

        public ImproveTryStrategy(int seed, double randomNeighborProb, double randomNewProb, double improveScoreProb, double improveSatisProb) : base(seed)
        {
            this.randomNeighborProb = randomNeighborProb;
            this.randomNewProb = randomNewProb;
            this.improveScoreProb = improveScoreProb;
            this.improveSatisProb = improveSatisProb;
        }

        public override bool Try(SimulatedAnnealingSolver solverInstance, ref SatConfiguration currentConfiguration)
        {
            SatConfiguration triedConfiguration;

            double randomChoice = random.NextDouble();

            if (randomChoice < randomNeighborProb)
                triedConfiguration = GetRandomNeighbor(solverInstance, currentConfiguration);
            else if (randomChoice < randomNeighborProb + randomNewProb)
                triedConfiguration = SatConfiguration.RandomConfiguration(solverInstance.SatInstance, random);
            else if (randomChoice < randomNeighborProb + randomNewProb + improveScoreProb)
                triedConfiguration = GetBetterScoreNeighbor(solverInstance, currentConfiguration);
            else
                triedConfiguration = GetBetterSatisfiabilityNeighbor(solverInstance, currentConfiguration);

            triedConfiguration.Score = solverInstance.Options.ScoreStrategy.CalculateScore(triedConfiguration, solverInstance);
            if (Accept(triedConfiguration, currentConfiguration, solverInstance.CurrentTemperature))
            {
                currentConfiguration = triedConfiguration;
                return true;
            }

            return false;
        }

        private SatConfiguration GetBetterSatisfiabilityNeighbor(SimulatedAnnealingSolver solverInstance, SatConfiguration currentConfiguration)
        {
            var candidateIndex = currentConfiguration.IndexOfRandomToImproveSatisfiability(random);

            if (candidateIndex > 0)
            {
                var triedConfiguration = new SatConfiguration(currentConfiguration);
                triedConfiguration.FlipValuation(candidateIndex);
                return triedConfiguration;
            }
            else
            {
                return GetBetterScoreNeighbor(solverInstance, currentConfiguration);
            }
        }

        private SatConfiguration GetBetterScoreNeighbor(SimulatedAnnealingSolver solverInstance, SatConfiguration currentConfiguration)
        {
            var triedConfiguration = new SatConfiguration(currentConfiguration);

            var flipIndex = currentConfiguration.IndexOfRandomToImproveScore(random);
            //Nothing to improve (all valuations set to 1), reset the configuration entirely
            if (flipIndex < 0)
                triedConfiguration = SatConfiguration.RandomConfiguration(solverInstance.SatInstance, random);
            else
                triedConfiguration.FlipValuation(flipIndex);
            return triedConfiguration;
        }

        private SatConfiguration GetRandomNeighbor(SimulatedAnnealingSolver solverInstance, SatConfiguration currentConfiguration)
        {
            var flipIndex = random.Next(0, solverInstance.SatInstance.Literals.Count);
            var triedConfiguration = new SatConfiguration(currentConfiguration);

            triedConfiguration.FlipValuation(flipIndex);

            return triedConfiguration;
        }
    }
}

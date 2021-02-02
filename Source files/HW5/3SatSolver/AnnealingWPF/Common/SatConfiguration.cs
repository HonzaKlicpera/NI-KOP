using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnnealingWPF.Common
{
    public class SatConfiguration
    {
        public IList<bool> Valuations { get; set; } = new List<bool>();
        public SatInstance Instance { get; set; }
        public float Score { get; set; }

        private List<int> unsatisfiedLiterals = new List<int>();
        private int idOfMostUnsatisfiedLiteral = -1;
        private int optimalizationValue = -1;
        private int numberOfUnsatisfiedClauses;
        private Satisfiability configurationSatisfiability = Satisfiability.UNKNOWN;

        public SatConfiguration() { }

        public SatConfiguration(SatConfiguration other)
        {
            Valuations = new List<bool>(other.Valuations);
            Instance = other.Instance;
        }

        public static SatConfiguration RandomConfiguration(SatInstance instance, Random random)
        {
            var satConfiguration = new SatConfiguration();
            satConfiguration.Instance = instance;
            for (int i = 0; i < instance.Literals.Count; i++)
            {
                satConfiguration.Valuations.Add(Convert.ToBoolean(random.Next(0, 2)));
            }

            return satConfiguration;
        }

        public void FlipValuation(int flipIndex)
        {
            Valuations[flipIndex] = !Valuations[flipIndex];
        }

        public int GetOptimalizationValue()
        {
            if(optimalizationValue < 0)
                optimalizationValue = Instance.Literals.Aggregate(0, (total, current) => 
                    Valuations[current.Id-1] ? total + current.Weight : total);

            return optimalizationValue;
        }

        public int IndexOfRandomToImproveScore(Random random)
        {
            //Converts the valuations to a list of indexes of valuations that are negative
            var negativeIds = Valuations.Select((valuation, index) => valuation ? -1 : index).Where(i => i > 0);

            if (negativeIds.Count() == 0)
                return -1;
            return negativeIds.ElementAtOrDefault(random.Next(negativeIds.Count())) - 1;
        }


        public int IndexOfRandomToImproveSatisfiability(Random random)
        {
            if (configurationSatisfiability == Satisfiability.UNKNOWN)
                CalculateSatisfiability();

            if (unsatisfiedLiterals.Count == 0)
                return -1;
            return unsatisfiedLiterals.ElementAt(random.Next(unsatisfiedLiterals.Count)) - 1;
        }


        public bool IsSatisfiable()
        {
            if(configurationSatisfiability == Satisfiability.UNKNOWN)
                CalculateSatisfiability();

            if (configurationSatisfiability == Satisfiability.SATISFIABLE)
                return true;
            else if (configurationSatisfiability == Satisfiability.UNSATISFIABLE)
                return false;
            throw new Exception();
        }

        public int NumberOfUnsatisfiedClauses()
        {
            if (configurationSatisfiability == Satisfiability.UNKNOWN)
                CalculateSatisfiability();

            return numberOfUnsatisfiedClauses;
        }

        private void CalculateSatisfiability()
        {
            numberOfUnsatisfiedClauses = 0;
            foreach(var clause in Instance.Clauses)
            {
                if (!clause.IsSatisfiable(this))
                    IncrementUnsatisfiedLiterals(clause);
            }

            if (numberOfUnsatisfiedClauses == 0)
                configurationSatisfiability = Satisfiability.SATISFIABLE;
            else
                configurationSatisfiability = Satisfiability.UNSATISFIABLE;

        }

        private void IncrementUnsatisfiedLiterals(SatClause clause)
        {
            foreach(var literal in clause.RatedLiterals)
            {
                unsatisfiedLiterals.Add(literal.Literal.Id);
            }
            numberOfUnsatisfiedClauses++;
        }

        private enum Satisfiability
        {
            UNKNOWN,
            SATISFIABLE,
            UNSATISFIABLE
        }
    }
}

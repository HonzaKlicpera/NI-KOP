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

        private int optimalizationValue = -1;
        private int numberOfUnsatisfiedClauses;
        private Satisfiability configurationSatisfiability = Satisfiability.UNKNOWN;

        public SatConfiguration() { }

        public SatConfiguration(SatConfiguration other)
        {
            Valuations = new List<bool>(other.Valuations);
            Instance = other.Instance;
        }

        public int GetOptimalizationValue()
        {
            if(optimalizationValue < 0)
                optimalizationValue = Instance.Literals.Aggregate(0, (total, current) => 
                    Valuations[current.Id-1] ? total + current.Weight : total);

            return optimalizationValue;
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
                    numberOfUnsatisfiedClauses++;
            }

            if (numberOfUnsatisfiedClauses == 0)
                configurationSatisfiability = Satisfiability.SATISFIABLE;
            else
                configurationSatisfiability = Satisfiability.UNSATISFIABLE;

        }

        private enum Satisfiability
        {
            UNKNOWN,
            SATISFIABLE,
            UNSATISFIABLE
        }
    }
}

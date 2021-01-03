using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Common
{
    public class SatClause
    {
        public List<SatRatedLiteral> RatedLiterals { get; private set; } = new List<SatRatedLiteral>();

        public SatClause(ICollection<SatRatedLiteral> ratedLiterals)
        {
            RatedLiterals.AddRange(ratedLiterals);
        }

        public bool IsSatisfiable(SatConfiguration configuration)
        {
            foreach (var ratedLiteral in RatedLiterals)
            {
                //XOR - the valuation must be true AND negation false, or valuation must be false AND negation true
                if (configuration.Valuations[ratedLiteral.Literal.Id - 1] ^ ratedLiteral.IsNegated)
                    return true;
            }
            return false;
        }
    }
}

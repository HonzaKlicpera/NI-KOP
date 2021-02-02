using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Common
{
    public class SatRatedLiteral
    {
        public bool IsNegated { get; private set; }
        public SatLiteral Literal { get; private set; }

        public SatRatedLiteral(bool isNegated, SatLiteral literal)
        {
            IsNegated = isNegated;
            Literal = literal;
        }
    }
}

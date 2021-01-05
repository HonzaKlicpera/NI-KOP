using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnnealingWPF.Common
{
    public class SatInstance
    {
        public int Id { get; set; }
        public List<SatLiteral> Literals { get; set; } = new List<SatLiteral>();
        public List<SatClause> Clauses { get; set; } = new List<SatClause>();

        private int sumOfWeights = -1;

        public int GetSumOfWeights()
        {
            if (sumOfWeights < 0)
                sumOfWeights = Literals.Aggregate(0, (acc, l) => acc + l.Weight);
            return sumOfWeights;
        }
    }
}

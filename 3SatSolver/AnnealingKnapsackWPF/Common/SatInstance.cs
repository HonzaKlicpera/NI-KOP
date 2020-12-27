using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Common
{
    public class SatInstance
    {
        public int Id { get; set; }
        public List<SatLiteral> Literals { get; set; } = new List<SatLiteral>();
        public List<SatClause> Clauses { get; set; } = new List<SatClause>();

    }
}

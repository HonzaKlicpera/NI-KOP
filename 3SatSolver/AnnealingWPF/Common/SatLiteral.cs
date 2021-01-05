using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Common
{
    public class SatLiteral
    {
        public int Id { get; private set; }
        public int Weight { get; private set; }

        public SatLiteral(int id, int weight)
        {
            this.Id = id;
            this.Weight = weight;
        }
    }
}

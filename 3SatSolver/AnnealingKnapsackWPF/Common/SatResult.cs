using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Common
{
    public class SatResult
    {
        public SatInstance SatInstance { get; set; }
        //Actual configuration of the result
        public SatConfiguration Configuration { get; set; }
        //Optimal configuration
        public ReferenceConfiguration OptimalConfiguration { get; set; }

        public string ResultLabel { get; set; }

        public IList<DataPoint> MovesHistory { get; set; }

        public ulong NumberOfSteps { get; set; }
        public double RunTimeMs { get; set; }
        public double Epsilon { get; set; }
        public int NumberOfUnsatisfiedClauses { get; set; }
    }
}

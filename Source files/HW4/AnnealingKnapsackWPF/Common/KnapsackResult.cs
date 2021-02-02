using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Common
{
    public class KnapsackResult
    {
        public KnapsackInstance KnapsackInstance { get; set; }
        //Actual configuration of the result
        public KnapsackConfiguration Configuration { get; set; }
        //Optimal configuration retrieved using B&B
        public KnapsackConfiguration OptimalConfiguration { get; set; }

        public IList<DataPoint> MovesHistory { get; set; }

        public ulong NumberOfSteps { get; set; }
        public double RunTimeMs { get; set; }
        public double Epsilon { get; set; }
        public string WatchedParameter { get; set; }
    }
}

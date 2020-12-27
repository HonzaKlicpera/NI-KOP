using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Common
{
    public class KnapsackResult
    {
        public KnapsackInstance KnapsackInstance { get; set; }
        public KnapsackConfiguration Configuration { get; set; }

        public ulong NumberOfSteps { get; set; }
        public double RunTimeMs { get; set; }
        public double Epsilon { get; set; }
        public string WatchedParameter { get; set; }
    }
}

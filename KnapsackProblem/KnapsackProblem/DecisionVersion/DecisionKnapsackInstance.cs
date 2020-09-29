using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using KnapsackProblem.Common;
using KnapsackProblem.Helpers;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionKnapsackInstance: KnapsackInstance
    {
        public int MinimalPrice { get; set; }
    }
}

using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion
{
    public class ConstructiveResult: AbstractResult<KnapsackInstance>
    {
        public KnapsackSolution Solution { get; set; }
    }
}

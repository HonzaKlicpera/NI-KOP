using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.ConstructiveVersion
{
    public class ConstructiveResult: AbstractResult<KnapsackInstance>
    {
        public KnapsackConfiguration Solution { get; set; }
    }
}

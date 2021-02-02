using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace KnapsackProblem.Common
{
    public class KnapsackReferenceSolution
    {
        public int Id { get; set; }
        public int Price { get; set; }

        public IList<bool> SolutionVector { get; set; }

    }
}

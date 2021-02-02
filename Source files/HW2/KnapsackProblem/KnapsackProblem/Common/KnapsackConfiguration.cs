using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Common
{
    public class KnapsackConfiguration
    {
        public int Price { get; set; }
        public int Weight { get; set; }

        public IList<bool> ItemVector { get; set; }
    }
}

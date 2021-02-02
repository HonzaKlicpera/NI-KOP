using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Common
{
    public class KnapsackConfiguration
    {
        public KnapsackConfiguration() { }
        public KnapsackConfiguration(KnapsackConfiguration other)
        {
            Price = other.Price;
            Weight = other.Weight;
            ItemVector = new List<bool>(other.ItemVector);
        }

        public int Price { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }

        public IList<bool> ItemVector { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.Common
{
    public abstract class KnapsackInstance
    {
        public int Id { get; set; }
        public int KnapsackSize { get; set; }

        public IList<KnapsackItem> Items { get; set; }

        public int GetPriceOfAllItems()
        {
            return Items.Aggregate(0, (acc, i) => i.Price + acc);
        }
    }
}

using KnapsackProblem.Common;
using KnapsackProblem.DecisionVersion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KnapsackProblemTests
{
    [TestClass]
    public class UnitTest1
    {
        public KnapsackInstance CreateKnapsackInstance()
        {
            var items = new List<KnapsackItem> { 
                new KnapsackItem { Price = 5, Weight = 10 },
                new KnapsackItem { Price = 20, Weight = 1},
                new KnapsackItem { Price = 80, Weight = 2},
                new KnapsackItem { Price = 1250, Weight = 100}
            };
            return new DecisionKnapsackInstance { Items = items};
        }
        [TestMethod]
        public void PriceOfAllItemsTest()
        {
            var knapsackInstance = CreateKnapsackInstance();
            Assert.AreEqual(knapsackInstance.GetPriceOfAllItems(), 1355);
        }
    }
}

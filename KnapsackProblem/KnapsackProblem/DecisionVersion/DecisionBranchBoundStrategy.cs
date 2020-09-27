using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionBranchBoundStrategy : IDecisionStrategy
    {
        public bool DoesSolutionExist(DecisionKnapsackInstance decisionVersion)
        {
            return DoesSolutionExist(decisionVersion.Items.Count, 0, 0, decisionVersion.GetPriceOfAllItems(), decisionVersion);
        }

        bool DoesSolutionExist(int itemsRemaining, int currentPrice, 
            int currentWeight, int maxRemainingPrice, DecisionKnapsackInstance decisionVersion)
        {
            if (currentPrice + maxRemainingPrice < decisionVersion.MinimalPrice)
                return false;

            if (itemsRemaining == 0)
                return currentWeight <= decisionVersion.KnapsackSize && currentPrice >= decisionVersion.MinimalPrice;

            KnapsackItem currentItem = decisionVersion.Items[itemsRemaining - 1];
            if (DoesSolutionExist(itemsRemaining - 1, currentPrice + currentItem.Price, currentWeight + currentItem.Weight, maxRemainingPrice, decisionVersion))
                return true;
            if (DoesSolutionExist(itemsRemaining - 1, currentPrice, currentWeight, maxRemainingPrice - currentItem.Price, decisionVersion))
                return true;
            return false;
        }
    }
}

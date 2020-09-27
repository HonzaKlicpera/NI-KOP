using System;
using System.Collections.Generic;
using System.Text;
using KnapsackProblem.Common;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionBruteForceStrategy : IDecisionStrategy
    {
        public bool DoesSolutionExist(DecisionKnapsackInstance decisionVersion)
        {
            return DoesSolutionExist(decisionVersion.Items.Count, 0, 0, decisionVersion);
        }

        bool DoesSolutionExist(int itemsRemaining, int currentPrice, int currentWeight, DecisionKnapsackInstance decisionVersion)
        {
            if (itemsRemaining == 0)
                return currentWeight <= decisionVersion.KnapsackSize && currentPrice >= decisionVersion.MinimalPrice;

            KnapsackItem currentItem = decisionVersion.Items[itemsRemaining - 1];
            if (DoesSolutionExist(itemsRemaining - 1, currentPrice + currentItem.Price, currentWeight + currentItem.Weight, decisionVersion))
                return true;
            if (DoesSolutionExist(itemsRemaining - 1, currentPrice, currentWeight, decisionVersion))
                return true;
            return false;
        }
    }
}

﻿using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionBranchBoundStrategy : DecisionStrategy
    {
        public override DecisionSolution Solve(DecisionKnapsackInstance knapsackInstance)
        {
            numberOfSteps = 0;

            bool permutationExists =  DoesSolutionExist(knapsackInstance.Items.Count, 0, 0, knapsackInstance.GetPriceOfAllItems(), knapsackInstance);

            return new DecisionSolution { KnapsackInstance = knapsackInstance, NumberOfSteps = numberOfSteps, PermutationExists = permutationExists };
        }

        bool DoesSolutionExist(int itemsRemaining, int currentPrice, 
            int currentWeight, int maxRemainingPrice, DecisionKnapsackInstance knapsackInstance)
        {
            numberOfSteps++;

            if (currentWeight > knapsackInstance.KnapsackSize)
                return false;
            if (currentPrice + maxRemainingPrice < knapsackInstance.MinimalPrice)
                return false;

            if (itemsRemaining == 0)
                return currentWeight <= knapsackInstance.KnapsackSize && currentPrice >= knapsackInstance.MinimalPrice;

            KnapsackItem currentItem = knapsackInstance.Items[itemsRemaining - 1];
            if (DoesSolutionExist(itemsRemaining - 1, currentPrice + currentItem.Price, currentWeight + currentItem.Weight, maxRemainingPrice, knapsackInstance))
                return true;
            if (DoesSolutionExist(itemsRemaining - 1, currentPrice, currentWeight, maxRemainingPrice - currentItem.Price, knapsackInstance))
                return true;
            return false;
        }
    }
}

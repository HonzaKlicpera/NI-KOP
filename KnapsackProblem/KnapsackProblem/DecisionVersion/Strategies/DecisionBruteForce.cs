using System;
using System.Collections.Generic;
using System.Text;
using KnapsackProblem.Common;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionBruteForce : DecisionStrategy
    {
        public override DecisionSolution Solve(DecisionKnapsackInstance knapsackInstance)
        {
            numberOfSteps = 0;

            bool permutationExists = DoesSolutionExist(knapsackInstance.Items.Count, 0, 0, knapsackInstance);

            return new DecisionSolution { KnapsackInstance = knapsackInstance, NumberOfSteps = numberOfSteps, PermutationExists = permutationExists };
        }

        bool DoesSolutionExist(int itemsRemaining, int currentPrice, int currentWeight, DecisionKnapsackInstance knapsackInstance)
        {
            numberOfSteps++;

            if (itemsRemaining == 0)
                return currentWeight <= knapsackInstance.KnapsackSize && currentPrice >= knapsackInstance.MinimalPrice;

            KnapsackItem currentItem = knapsackInstance.Items[itemsRemaining - 1];
            if (DoesSolutionExist(itemsRemaining - 1, currentPrice + currentItem.Price, currentWeight + currentItem.Weight, knapsackInstance))
                return true;
            if (DoesSolutionExist(itemsRemaining - 1, currentPrice, currentWeight, knapsackInstance))
                return true;
            return false;
        }
    }
}

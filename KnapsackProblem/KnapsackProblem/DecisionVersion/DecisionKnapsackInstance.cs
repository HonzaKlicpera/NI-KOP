using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using KnapsackProblem.Common;
using KnapsackProblem.Helpers;

namespace KnapsackProblem.DecisionVersion
{
    public class DecisionKnapsackInstance: KnapsackInstance
    {
        public int MinimalPrice { get; set; }

        public DecisionKnapsackInstance() { }

        public DecisionKnapsackInstance(string inputLine)
        {
            ParseInputLine(inputLine);
        }

        public bool DoesSolutionExist(IDecisionStrategy solutionStrategy)
        {
            return solutionStrategy.DoesSolutionExist(this);
        }

        void ParseInputLine(string inputLine)
        {
            var inputFields = inputLine.Split(' ');

            Id = -InputFieldParser.ParseIntField(inputFields[0], "Id");
            KnapsackSize = InputFieldParser.ParseNonNegativeIntField(inputFields[2], "Knapsack size");
            MinimalPrice = InputFieldParser.ParseNonNegativeIntField(inputFields[3], "Minimal price");
            Items = InputFieldParser.ParseItems(inputFields.Skip(4).ToArray());
        }
    }
}

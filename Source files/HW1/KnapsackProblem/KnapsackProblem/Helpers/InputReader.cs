using KnapsackProblem.Common;
using KnapsackProblem.DecisionVersion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KnapsackProblem.Helpers
{
    public static class InputReader
    {
        public static IList<DecisionKnapsackInstance> ReadDecisionKnapsackInstances(string location)
        {

            var instances = new List<DecisionKnapsackInstance>();
            using (StreamReader file = new StreamReader(location))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var instance = InputFieldParser.ParseDecisionKnapsackInstance(ln);
                    instances.Add(instance);
                }
            }
            return instances;
        }

        public static Dictionary<int, KnapsackReferenceSolution> ReadReferenceSolutions(string location)
        {
            var solutions = new Dictionary<int, KnapsackReferenceSolution>();
            using (StreamReader file = new StreamReader(location))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var solution = InputFieldParser.ParseSolution(ln);
                    solutions[solution.Id] = solution;
                }
            }
            return solutions;
        }
    }
}

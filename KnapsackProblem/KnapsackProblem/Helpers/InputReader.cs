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
        public static IList<T> ReadKnapsackInstances<T>(string location, Func<string, T> instanceParser) where T: KnapsackInstance
        {
            var instances = new List<T>();
            using (StreamReader file = new StreamReader(location))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var instance = instanceParser(ln);
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

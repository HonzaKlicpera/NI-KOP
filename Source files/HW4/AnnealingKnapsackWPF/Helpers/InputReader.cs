using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KnapsackProblem.Helpers
{
    public static class InputReader
    {
        public static IList<KnapsackInstance> ReadKnapsackInstances(string location)
        {
            var instances = new List<KnapsackInstance>();
            using (StreamReader file = new StreamReader(location))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var instance = InputFieldParser.ParseConstructiveKnapsackInstance(ln);
                    instances.Add(instance);
                }
            }
            return instances;
        }
    }
}

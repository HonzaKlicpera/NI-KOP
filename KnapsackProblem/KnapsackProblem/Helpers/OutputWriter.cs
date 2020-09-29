using CsvHelper;
using CsvHelper.Configuration;
using KnapsackProblem.DecisionVersion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace KnapsackProblem.Helpers
{
    public sealed class SingleResultMap: ClassMap<DecisionSolution>
    {
        public SingleResultMap()
        {
            Map(m => m.KnapsackInstance.Id).Name("ID");
            Map(m => m.NumberOfSteps).Name("Number of steps");
            Map(m => m.PermutationExists).Name("Result");
            Map(m => m.KnapsackInstance.Items.Count).Name("n");
        }
    }
    public static class OutputWriter
    {
        public static void WriteAllResults(IList<DecisionSolution> solutions, string location)
        {
            bool fileExists = File.Exists(location);

            using (var stream = File.Open(location, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Don't write the header if file already existed.
                csv.Configuration.RegisterClassMap<SingleResultMap>();
                csv.Configuration.HasHeaderRecord = !fileExists;
                csv.WriteRecords(solutions);
            }
        }


    }
}

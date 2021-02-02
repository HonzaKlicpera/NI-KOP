using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AnnealingWPF.Helpers
{
    
    public sealed class ResultMap: ClassMap<SatResult>
    {
        public ResultMap()
        {
            Map(m => m.SatInstance.Id).Name("ID");
            Map(m => m.RunTimeMs).Name("Average Runtime [ms]");
            Map(m => m.RestartCount).Name("Restart count");
            Map(m => m.NumberOfSteps).Name("Number of steps");
            Map(m => m.Configuration.Valuations).TypeConverter<ItemVectorConverter>().Name("Result vector");
            Map(m => m.SatInstance.Literals.Count).Name("n");
            Map(m => m.Epsilon).Name("Epsilon");
            Map(m => m.NumberOfUnsatisfiedClauses).Name("Number of unsatisfied clauses");
            Map(m => m.BestConfigurationFoundAt).Name("Best configuration found at");
            Map(m => m.ResultLabel).Name("Result Label");
        }
    }

    public class ItemVectorConverter: DefaultTypeConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return OutputWriter.ItemVectorToString((IList<bool>) value);
        }
    }

    public static class OutputWriter
    {

        public static void WriteAllResults(IList<SatResult> results, string location)
        {
            bool fileExists = File.Exists(location);

            using (var stream = File.Open(location, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Don't write the header if file already existed.
                csv.Configuration.RegisterClassMap<ResultMap>();
                csv.Configuration.HasHeaderRecord = !fileExists;
                csv.WriteRecords(results);
                csv.Flush();
            }
        }

        public static string ItemVectorToString(IList<bool> itemVector)
        {
            return itemVector.Aggregate(new StringBuilder(), ItemVectorAggregator).ToString();
        }

        private static StringBuilder ItemVectorAggregator(StringBuilder acc, bool current)
        {
            acc.Append(current ? "1" : "0");
            acc.Append(" ");
            return acc;
        }
    }
    
}

﻿using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using KnapsackProblem.Common;
using KnapsackProblem.ConstructiveVersion;
using KnapsackProblem.DecisionVersion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace KnapsackProblem.Helpers
{
    public sealed class DecisionResultMap: ClassMap<DecisionResult>
    {
        public DecisionResultMap()
        {
            Map(m => m.KnapsackInstance.Id).Name("ID");
            Map(m => m.NumberOfSteps).Name("Number of steps");
            Map(m => m.PermutationExists).Name("Result");
            Map(m => m.KnapsackInstance.Items.Count).Name("n");
            Map(m => m.Strategy).Name("Strategy");
            Map(m => m.DataSetName).Name("Data set name");
        }
    }

    public sealed class ConstructiveResultMap: ClassMap<ConstructiveResult>
    {
        public ConstructiveResultMap()
        {
            Map(m => m.KnapsackInstance.Id).Name("ID");
            Map(m => m.RunTimeMs).Name("Average Runtime [ms]");
            Map(m => m.Configuration.ItemVector).TypeConverter<ItemVectorConverter>().Name("Result vector");
            Map(m => m.KnapsackInstance.Items.Count).Name("n");
            Map(m => m.Epsilon).Name("Epsilon");
            Map(m => m.Strategy).Name("Strategy"); 
            Map(m => m.DataSetName).Name("Data set name");
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
        public static void WriteDecisionResults(IList<DecisionResult> results, string location)
        {
            WriteAllResults<DecisionResult, DecisionResultMap>(results, location);
        }

        public static void WriteConstructiveResults(IList<ConstructiveResult> results, string location)
        {
            WriteAllResults<ConstructiveResult, ConstructiveResultMap>(results, location);
        }

        public static void WriteAllResults<R, M>(IList<R> results, string location)
            where M: ClassMap<R> 
        {
            bool fileExists = File.Exists(location);

            using (var stream = File.Open(location, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Don't write the header if file already existed.
                csv.Configuration.RegisterClassMap<M>();
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

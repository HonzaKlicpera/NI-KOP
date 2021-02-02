using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;
using KnapsackProblem.Common;
using KnapsackProblem.Exceptions;

namespace KnapsackProblem.Helpers
{

    public static class InputFieldParser
    {
        public static int ParseIntField(string field, string fieldName)
        {
            int value;
            try
            {
                value = int.Parse(field);
            }
            catch (ArgumentException)
            {
                throw new InvalidInputFormatException($"{fieldName} is not an int");
            }

            return value;
        }

        public static int ParseNonNegativeIntField(string field, string fieldName)
        {
            var value = ParseIntField(field, fieldName);

            if (value < 0)
                throw new InvalidInputFormatException($"{fieldName} is negative");

            return value;
        }

        public static KnapsackInstance ParseConstructiveKnapsackInstance(string inputLine)
        {
            var inputFields = inputLine.Split(' ');

            var id = ParseIntField(inputFields[0], "Id");
            var knapsackSize = ParseNonNegativeIntField(inputFields[2], "Knapsack size");
            var items = ParseItems(inputFields.Skip(3).ToArray());

            return new KnapsackInstance { Id = id, KnapsackSize = knapsackSize, Items = items };

        }

        public static List<bool> ParseSolutionVector(string[] fields)
        {
            var solutionVector = new List<bool>();

            foreach (var f in fields)
            {
                if (f == "1")
                    solutionVector.Add(true);
                else if (f == "0")
                    solutionVector.Add(false);
                else if (f != "")
                    throw new InvalidInputFormatException("The solution vector can be defined only using 0/1 values");
            }

            return solutionVector;
        }

        public static IList<KnapsackItem> ParseItems(string[] fields)
        {
            if (fields.Length % 2 != 0)
                throw new InvalidInputFormatException("Price of the last item is not defined");

            var items = new List<KnapsackItem>();

            for (int i = 0; i < fields.Length; i += 2)
            {
                items.Add(ParseItem(fields[i], fields[i + 1], i/2));
            }

            return items;
        }

        public static KnapsackItem ParseItem(string weightField, string priceField, int itemId)
        {
            int price = ParseNonNegativeIntField(priceField, "Item price");
            int weight = ParseNonNegativeIntField(weightField, "Item weight");

            return new KnapsackItem { Price = price, Weight = weight, Id = itemId };
        }
    }
}

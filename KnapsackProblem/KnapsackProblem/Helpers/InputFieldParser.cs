using System;
using System.Collections.Generic;
using System.Text;
using KnapsackProblem.Common;

namespace KnapsackProblem.Helpers
{
    class InvalidInputFormatException : Exception
    {
        public InvalidInputFormatException() { }
        public InvalidInputFormatException(string message) : base(message) { }
    }

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

        public static IList<KnapsackItem> ParseItems(string[] fields)
        {
            if (fields.Length % 2 != 0)
                throw new InvalidInputFormatException("Price of the last item is not defined");

            var items = new List<KnapsackItem>();

            for (int i = 0; i < fields.Length; i += 2)
            {
                items.Add(ParseItem(fields[i], fields[i + 1]));
            }

            return items;
        }

        public static KnapsackItem ParseItem(string weightField, string priceField)
        {
            int price = ParseNonNegativeIntField(priceField, "Item price");
            int weight = ParseNonNegativeIntField(weightField, "Item weight");

            return new KnapsackItem { Price = price, Weight = weight };
        }
    }
}

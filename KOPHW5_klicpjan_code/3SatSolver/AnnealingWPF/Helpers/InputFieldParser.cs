using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;
using AnnealingWPF.Common;
using AnnealingWPF.Exceptions;
using System.IO;
using System.Text.RegularExpressions;

namespace AnnealingWPF.Helpers
{

    public static class InputFieldParser
    {
        public const int CLAUSE_LENGTH = 3;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="clauseLength">Length of the clause, 0 stands for variable length</param>
        /// <returns></returns>
        public static SatClause ParseSatClause(string line, IList<SatLiteral> literals, int clauseLength = 0)
        {
            var splitLine = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                
            if (clauseLength > 0 && splitLine.Length - 1 != clauseLength)
                throw new InvalidInputFormatException("Invalid number of literals in a clause");
            if (!int.TryParse(splitLine.Last(), out int result) || result != 0)
                throw new InvalidArgumentException("Last number in a clause is not 0");

            var ratedLiterals = new List<SatRatedLiteral>();

            foreach (var literalId in splitLine.Take(splitLine.Length - 1))
            {
                if (int.TryParse(literalId, out int parsedId))
                    ratedLiterals.Add(new SatRatedLiteral(parsedId < 0, literals[Math.Abs(parsedId)-1]));
                else
                    throw new InvalidArgumentException("Could not parse literal id in a clause");
            }
            return new SatClause(ratedLiterals);
        }

        public static List<SatLiteral> ParseLiteralWeights(string line, int expectedLiteralCount)
        {
            var splitLine = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            if (splitLine.Length - 2 != expectedLiteralCount)
                throw new InvalidInputFormatException("Number of expected literals does not match number of weight parameters");
            var test = splitLine.Skip(1)
                .SkipLast(1).ToList();

            return splitLine.Skip(1)
                .SkipLast(1)
                .Select((weight, index) => new SatLiteral(index+1, int.Parse(weight)))
                .ToList();
        }

        public static void ParseInstanceInfoLine(string line, out int numberOfLiterals, out int numberOfClauses)
        {
            var splitLine = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            if (splitLine[1] != "mwcnf")
                throw new InvalidInputFormatException("p definition line contains incorrect name (should be mwcnf)");

            if (!int.TryParse(splitLine[2], out numberOfLiterals))
                throw new InvalidInputFormatException("could not parse number of literals");
            if (!int.TryParse(splitLine[3], out numberOfClauses))
                throw new InvalidInputFormatException("could not parse number of clauses");
        }

        public static int ParseInstanceId(string fileName)
        {
            string pattern = @"\-\d+\.";
            Regex rg = new Regex(pattern);

            var match = rg.Match(fileName);

            if (!match.Success)
                throw new Exception("File name is in invalid format");

            return int.Parse(Regex.Replace(match.Value, "[^0-9]", ""));
        }

        public static ReferenceConfiguration ParseOptimalConfiguration(string line)
        {
            var splitLine = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            if (!int.TryParse(splitLine.Last(), out int result) || result != 0)
                throw new InvalidArgumentException("Last number in the definition is not 0");

            var instanceSize = int.Parse(Regex.Replace(Regex.Match(splitLine[0], @"\d+\-").Value, "[^0-9]", ""));
            var instanceId = int.Parse(Regex.Replace(Regex.Match(splitLine[0], @"\-\d+").Value, "[^0-9]", ""));

            if (instanceSize != splitLine.Length - 3)
                throw new InvalidArgumentException("Invalid number of literals hurr durr");

            return new ReferenceConfiguration
            {
                InstanceId = instanceId,
                OptimalizationValue = ParseNonNegativeIntField(splitLine[1], "optimalization value"),
                Valuations = splitLine.Skip(2).SkipLast(1).Select(id => ParseIntField(id, "id") > 0).ToList()
            };
            
        }
    }
}

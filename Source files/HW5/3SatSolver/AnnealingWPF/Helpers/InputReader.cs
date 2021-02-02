using AnnealingWPF.Common;
using AnnealingWPF.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AnnealingWPF.Helpers
{
    public static class InputReader
    {
        public static IList<SatInstance> ReadSatInstances(string location)
        {
            var instances = new List<SatInstance>();

            foreach (var filePath in Directory.GetFiles(location, "*.mwcnf"))
            {
                instances.Add(ReadSatInstance(filePath));
            }

            return instances;
        }

        public static IDictionary<int, ReferenceConfiguration> ReadOptimalConfigurations(string filePath)
        {
            var optimalConfigurations = new Dictionary<int, ReferenceConfiguration>();
            using (StreamReader file = new StreamReader(filePath))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var trimmedLn = ln.Trim();
                    if (trimmedLn != "")
                    {
                        var configuration = InputFieldParser.ParseOptimalConfiguration(trimmedLn);
                        optimalConfigurations.TryAdd(configuration.InstanceId, configuration);
                    }

                }
            }
            if (optimalConfigurations.Count == 0)
                throw new InvalidInputFormatException("The reference solution file does not contain any valid values");

            return optimalConfigurations;
        }


        public static SatInstance ReadSatInstance(string instanceFileLocation)
        {
            var fileName = Path.GetFileName(instanceFileLocation);
            var clauses = new List<SatClause>();
            var instance = new SatInstance();
            int numberOfLiterals = 0;
            int numberOfClauses = 0;
            instance.Id = InputFieldParser.ParseInstanceId(fileName);

            using (StreamReader file = new StreamReader(instanceFileLocation))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var trimmedLn = ln.Trim();
                    if (trimmedLn.StartsWith('p'))
                        InputFieldParser.ParseInstanceInfoLine(trimmedLn, out numberOfLiterals, out numberOfClauses);
                    else if (trimmedLn.StartsWith('w'))
                        instance.Literals = InputFieldParser.ParseLiteralWeights(trimmedLn, numberOfLiterals);
                    else if (trimmedLn.StartsWith('c'))
                        continue;
                    else if (trimmedLn.StartsWith('%'))
                        break;
                    else
                       clauses.Add(InputFieldParser.ParseSatClause(trimmedLn, instance.Literals, InputFieldParser.CLAUSE_LENGTH));
                }

                if (clauses.Count != numberOfClauses)
                    Debug.WriteLine($"Warning: Number of expected clauses({numberOfClauses}) does not match the actual amount({clauses.Count}) - {fileName}");
                instance.Clauses = clauses;
            }
            return instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using KnapsackProblem.Common;
using KnapsackProblem.DecisionVersion;
using KnapsackProblem.Helpers;
using CommandLine;
using KnapsackProblem.Exceptions;

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(
                o =>
                {
                    if (o.ProblemVersion.Equals("decision", StringComparison.OrdinalIgnoreCase)
                        || o.ProblemVersion.Equals("d", StringComparison.OrdinalIgnoreCase))
                    {
                        var strategy = GetDecisionStrategy(o.Strategy);
                        var instances = ReadDecisionKnapsackInstances(o.InputFile);

                        var solutions = strategy.SolveAll(instances);

                        if(o.ReferenceFile != null)
                        {
                            Console.WriteLine("Validating the results...");
                            var referenceSolutions = ReadReferenceSolutions(o.ReferenceFile);
                            ValidateSolutions(solutions, referenceSolutions);
                        }
                        //TODO: properly check whether to write into file or console
                        //WriteResults(solutions, o.OutputFile);
                        OutputWriter.WriteAllResults(solutions, o.OutputFile);
                    }
                });

            /*
            bool checkValidity = true;
            Dictionary<int, int> correctAnswers = new Dictionary<int, int>();
            IDecisionKnapsackStrategy strategy = new DecisionBruteForceStrategy();

            if (checkValidity)
            {
                correctAnswers = ParseCorrectAnswers("C:\\Downloads\\NR\\NK20_sol.dat");
            }
            using (StreamReader file = new StreamReader("C:\\Downloads\\NR\\NR20_inst.dat"))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    var dec = new DecisionKnapsackInstance(ln);
                    var result = dec.DoesSolutionExist(strategy);
                    Console.WriteLine(dec.Id + " " + result);

                    if (checkValidity)
                    {
                        if (dec.MinimalPrice <= correctAnswers[dec.Id] == result)
                            Console.WriteLine("All good!");
                        else
                            Console.WriteLine("noooooo");
                    }
                }
                
            }
            */
        }

        //TODO: Move methods into proper class
        static void ValidateSolutions(IList<DecisionSolution> solutions, Dictionary<int, KnapsackReferenceSolution> referenceSolutions)
        {
            bool allCorrect = true;
            foreach (var s in solutions)
            {
                var referenceSolution = referenceSolutions[s.KnapsackInstance.Id];
                var shouldPermutationExist = referenceSolution.Price >= s.KnapsackInstance.MinimalPrice;

                if (s.PermutationExists != shouldPermutationExist)
                {
                    Console.WriteLine($"Permutation instance solution (id {s.KnapsackInstance.Id}) incorrect," +
                        $" result: {s.PermutationExists}, expected result: {shouldPermutationExist}");
                    allCorrect = false;
                }
            }
            if (allCorrect)
                Console.WriteLine("All solutions match the reference data");
            else
                Console.WriteLine("Not all solutions match the reference data, see the log above for more details");
        }

        static DecisionStrategy GetDecisionStrategy(string strategyField)
        {
            if (strategyField.Equals("bruteforce", StringComparison.OrdinalIgnoreCase))
                return new DecisionBruteForceStrategy();
            else if (strategyField.Equals("BranchAndBound", StringComparison.OrdinalIgnoreCase))
                return new DecisionBranchBoundStrategy();
            throw new InvalidArgumentException($"{strategyField} is not a valid strategy for decision version");
        }

        static IList<DecisionKnapsackInstance> ReadDecisionKnapsackInstances(string location)
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

        static void WriteResults(IList<DecisionSolution> solutions, string location)
        {
            int acc = 0;
            foreach(var s in solutions)
            {
                Console.WriteLine($"Id:{s.KnapsackInstance.Id}, Number of steps:{s.NumberOfSteps}, result: {s.PermutationExists}");
                acc += s.NumberOfSteps;
            }

            Console.WriteLine($"Total number of steps:{acc}");
        }


        static Dictionary<int, KnapsackReferenceSolution> ReadReferenceSolutions(string location)
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

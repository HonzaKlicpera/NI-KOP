using KnapsackProblem.Exceptions;
using KnapsackProblem.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KnapsackProblem.Common
{
    public abstract class AbstractVersionHandler<R, I>
    {
        public abstract void HandleData(CommandLineOptions options);

        protected bool ValidateResults<R, I>(IList<R> results, string referenceFile, Func<R, KnapsackReferenceSolution, bool> comparator)
            where I : KnapsackInstance where R : AbstractResult<I>
        {
            Console.WriteLine("Validating the results...");
            try
            {
                var referenceSolutions = InputReader.ReadReferenceSolutions(referenceFile);
                SolutionValidator.ValidateResults<R, I>(results, referenceSolutions, comparator);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Could not open reference instances file at {referenceFile} ({e.Message})");
                return false;
            }
            catch (InvalidInputFormatException e)
            {
                Console.WriteLine($"Could not parse the reference instances file: {e.Message}");
                return false;
            }
            return true;
        }

        protected bool ProcessInputInstances<T>(out IList<T> instances, string inputFile, Func<string, T> instanceParser) 
            where T : KnapsackInstance
        {
            instances = new List<T>();
            //Open and parse the input instances
            Console.WriteLine($"Opening file {inputFile}");
            try
            {
                instances = InputReader.ReadKnapsackInstances(inputFile, instanceParser);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Could not open input instances file at {inputFile} ({e.Message})");
                return false;
            }
            catch (InvalidInputFormatException e)
            {
                Console.WriteLine($"Could not parse the input instances file: {e.Message}");
                return false;
            }
            return true;
        }
    }
}

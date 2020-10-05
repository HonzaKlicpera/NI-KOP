using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace KnapsackProblem.Helpers
{
    public class CommandLineOptions
    {
        [Option('i', "input", Required = true, HelpText = "File containing the input data")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file location")]
        public string OutputFile { get; set; }

        [Option('t', "test", HelpText = "Test file location")]
        public string ReferenceFile { get; set; }

        [Option('p', "problem", Required = true, HelpText = "The version of the knapsack problem")]
        public string ProblemVersion { get; set; }

        [Option('s', "strategy", Required = true, HelpText = "The algorithmic strategy for solving the problem")]
        public string Strategy { get; set; }

        [Option("setname", Default = "", HelpText = "The input data set name")]
        public string DataSetName { get; set; }
    }
}

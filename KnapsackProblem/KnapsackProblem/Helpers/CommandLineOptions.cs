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

        [Option('o', "output", HelpText = "Output file location")]
        public string OutputFile { get; set; }

        [Option('t', "test", HelpText = "Test file location")]
        public string ReferenceFile { get; set; }

        [Option('p', "problem", Required = true, HelpText = "The problem version for the solver")]
        public string ProblemVersion { get; set; }

        [Option('s', "strategy", Required = true, HelpText = "The algorithmic strategy for solving the problem")]
        public string Strategy { get; set; }
    }
}

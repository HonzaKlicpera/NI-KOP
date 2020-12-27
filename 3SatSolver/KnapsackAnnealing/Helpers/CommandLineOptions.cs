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

    }
}

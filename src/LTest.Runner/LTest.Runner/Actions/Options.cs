using System.Collections.Generic;
using CommandLine;

namespace LTest.Runner.Actions
{
    [Verb("farm")]
    public class Options
    {
        [Option('r', "read", Required = true, HelpText = "Input files to be processed.")]
        public IEnumerable<string> InputFiles { get; set; }

      
        [Option("stdin",Default = false, HelpText = "Read from stdin")]
        public bool stdin { get; set; }

        [Value(0, MetaName = "offset", HelpText = "File offset.")]
        public long? Offset { get; set; }
    }
}

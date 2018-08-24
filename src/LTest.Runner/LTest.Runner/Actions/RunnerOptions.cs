using System.Collections.Generic;
using CommandLine;

namespace LTest.Runner.Actions
{
    [Verb("run")]
    public class RunnerOptions
    {
        [Option('p', "path", Required = true, HelpText = "Input folder to be processed.",Separator = ',')]
        public IEnumerable<string> InputFiles { get; set; }

        [Option('t', "timestorun", Required = false, HelpText = "This is the pram for number of times to run the test (Default: 1000)",Default = 1000)]
        public int TimesToRun { get; set; }

        [Option('d', "delay", Required = false, HelpText = "This is the pram for delay between requests (Default: 0ms)", Default = 0)]
        public int Delay { get; set; }

    }
}
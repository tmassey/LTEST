using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using LTest.Core.Interfaces;
using LTest.Core.Logic;
using LTest.Runner.Actions;
//using LTest.Runner.CommandLine;

namespace LTest.Runner
{
    internal class Program
    {
        private static readonly List<Assembly> _assymblies = new List<Assembly>();

        //TODO: Refactor this whole thing its ugly
        //TODO: Write some tests and stop spiking the code out come on man TDD
        private static void Main(string[] args)
        {
            Display.ClearScreen(ConsoleColor.Blue, ConsoleColor.White);

            Parser.Default.ParseArguments<RunnerOptions>(args)
                .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed(errs => HandleParseError(errs));

            //Run();
        }

        private static void HandleParseError(object errs)
        {
            Console.WriteLine("Press Any key To Continue...");
            Console.ReadLine();
        }

        private static void RunOptionsAndReturnExitCode(object opts)
        {
            var options = (RunnerOptions) opts;
            var paths = options.InputFiles;
            Run(paths.ToList(), options.TimesToRun, options.Delay);
            //Console.WriteLine("Press Any key To Continue...");
            //Console.ReadLine();
        }

        private static void Run(List<string> folders, int timesToRun, int delay)
        {
            var manager = new ThreadManager(timesToRun, delay);
            foreach (var path in folders)
            {
                var assymblies = GetAssymblies(path);
                LoadAssymblies(assymblies);
                BuildWorkers(manager);
            }

            manager.RunWorkers();
        }

        private static IEnumerable<string> GetAssymblies(string path)
        {
            return Directory.EnumerateFiles(path)
                .Where(x => x.EndsWith(".dll") || x.EndsWith(".exe"));
        }

        private static void BuildWorkers(ThreadManager manager)
        {
            var allTestTypes = GetAllTestTypes();

            foreach (var test in allTestTypes)
            {                
                manager.AddWorker(test);
            }
        }

        private static void LoadAssymblies(IEnumerable<string> assymblies)
        {
            foreach (var assymbly in assymblies)
            {
                var asym = Assembly.LoadFile(assymbly);
                _assymblies.Add(asym);
            }
        }


        private static List<string> GetAllTestTypes()
        {
            return _assymblies.SelectMany(x => x.GetTypes())
                .Where(x => x.BaseType!=null && x.BaseType.Name=="LoadTest")
                .Select(x => x.Name).ToList();
        }
    }
}
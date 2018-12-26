using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CommandLine;
using LTest.Runner.Actions;
using LTest.Runner.Interfaces;


namespace LTest.Runner
{
    internal class Program
    {
        private static readonly List<Assembly> _assymblies = new List<Assembly>();
        private static RunnerOptions _options;
   
        private static TimeSpan _totalTestTime;
        private static bool _anyFailures;
        private static readonly List<string> _failedTests = new List<string>();
        private static int _totalTests;
        private static Dictionary<string,Double> _maxExecutionTime  = new Dictionary<string, Double>();
        private static List<TestEndHandlerArgs> _testResults= new List<TestEndHandlerArgs>();

        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<RunnerOptions>(args)
                .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed(errs => HandleParseError(errs));
        }


        private static void HandleParseError(object errs)
        {
            Console.WriteLine("Press Any key To Continue...");
            Console.ReadLine();
        }

        private static void RunOptionsAndReturnExitCode(object opts)
        {
            _options = (RunnerOptions) opts;

            var paths = _options.InputFiles;
            Run(paths.ToList(), _options.TimesToRun, _options.Delay);
            Console.WriteLine("Press Any key To Continue...");
            Console.ReadLine();
        }

        private static void Run(List<string> folders, int timesToRun, int delay)
        {
            var manager = new ThreadManager(timesToRun, delay);
            manager.OnProgressChanged += Manager_OnProgressChanged;
            manager.OnTestEnd += Manager_OnTestEnd;
            foreach (var path in folders)
            {
                var assymblies = GetAssymblies(path);
                LoadAssymblies(assymblies);
                BuildWorkers(manager);
            }

            manager.RunWorkers();
            ReportOnLoadTests();
        }

        private static void ReportOnLoadTests()
        {
            if (_anyFailures)
            {
                Console.WriteLine("Tests Failed:");
                foreach (var test in _failedTests) Console.WriteLine(test + " Failed!");
            }
            else
            {
                Console.WriteLine("Tests all Passed!");
                Console.WriteLine("Average Test Time: " + _totalTestTime.TotalMilliseconds / _totalTests + "ms");
                foreach (var time in _maxExecutionTime)
                {
                    Console.WriteLine("Max Test Time for " + time.Key + ":" + time.Value + "ms");
                }
            }
            Console.WriteLine("Total Success:" +_testResults.Count(x=>x.IsSuccess));
            Console.WriteLine("Total Failures:" + _testResults.Count(x => !x.IsSuccess));
            foreach (var test in _testResults.Select(x=>x.TestName).Distinct())
            {
                Console.WriteLine(test);
                Console.WriteLine(test + " Success:" + _testResults.Count(x => x.IsSuccess && x.TestName == test));
                Console.WriteLine(test + " Failures:" + _testResults.Count(x => !x.IsSuccess && x.TestName==test));
            }
        }

        private static void Manager_OnTestEnd(object sender, TestEndHandlerArgs args)
        {
            var message = "";
            _testResults.Add(args);
            if (args.IsSuccess)
            {
                _totalTestTime += args.TimeForTest;
                _totalTests += 1;
                if(!_maxExecutionTime.ContainsKey(args.TestName))
                    _maxExecutionTime.Add(args.TestName,0);
                if (_maxExecutionTime[args.TestName] < args.TimeForTest.TotalMilliseconds)
                    _maxExecutionTime[args.TestName] = args.TimeForTest.TotalMilliseconds;
                message = args.TestName + ": Success in " + _totalTestTime.Milliseconds + "ms      ";
                Console.WriteLine(message);
            }
            else
            {
                _anyFailures = true;
                _failedTests.Add(args.TestName);
                Console.Write("#");
            }

            
        }

        private static void Manager_OnProgressChanged(object sender, ProgressChangedArgs args)
        {
            Console.Write(".");
        }


        private static IEnumerable<string> GetAssymblies(string path)
        {
            return Directory.EnumerateFiles(path)
                .Where(x => x.EndsWith(".dll") || x.EndsWith(".exe"));
        }

        private static void BuildWorkers(ThreadManager manager)
        {
            var allTestTypes = GetAllTestTypes();

            foreach (var test in allTestTypes) manager.AddWorker(test);
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
                .Where(x => x.BaseType != null && x.BaseType.Name == "LoadTest")
                .Select(x => x.Name).ToList();
        }
    }
}
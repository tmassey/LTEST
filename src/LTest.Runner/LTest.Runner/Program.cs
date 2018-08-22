using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTest.Core.Interfaces;
using LTest.Core.Logic;

namespace LTest.Runner
{
    class Program
    {

        //TODO: Refactor this whole thing its ugly
        //TODO: Write some tests and stop spiking the code out come on man TDD
        static void Main(string[] args)
        {
            var path = @"C:\os-projects\LTEST\src\LTest.Core\bin\Debug";

            var assymblies = System.IO.Directory.EnumerateFiles(path).Where(x => x.EndsWith(".dll") || x.EndsWith(".exe"));
            foreach (var assymbly in assymblies)
            {
                var asym = System.Reflection.Assembly.LoadFile(assymbly);
            }
            var allTestTypes = GetAllTestTypes();
            foreach (var test in allTestTypes)
            {
                //TODO: Make this multithreaded and look into how we can do a LoadTest farm of servers
                var loadTest = GetInstance(test);
                loadTest.BeforeTest();
                loadTest.Execute();
                loadTest.AfterTest();
            }

            Console.WriteLine("Press Any key To Continue...");
            Console.ReadLine();
        }
        public static LoadTest GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return (LoadTest)Activator.CreateInstance(type);
            var asymblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asymblies)
            {
                var types = asm.GetTypes();
                foreach (var foundType in types)
                {
                    if(foundType.BaseType != null && (foundType.BaseType.FullName== "LTest.Core.Logic.LoadTest" && foundType.FullName.Contains(strFullyQualifiedName)))
                        return (LoadTest)Activator.CreateInstance(foundType);
                }
            }
            return null;
        }
        private static List<String> GetAllTestTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                            .Where(x => typeof(LoadTest).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                            .Select(x => x.Name).ToList();
        }
    }
}

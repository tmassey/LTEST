using System;
using System.IO;
using System.Reflection;
using LTest.Core.Interfaces;
using LTest.Core.Logic;
using LTest.Runner.Interfaces;

namespace LTest.Runner
{
    public class Worker : IWorker
    {
        private readonly ILoadTest _test;

        public Worker(string testType)
        {
            _test = GetInstance(testType);
        }

        public void RunTest()
        {
            _test.BeforeTest();
            _test.Execute();
            _test.AfterTest();
        }

        private static ILoadTest GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return (ILoadTest)Activator.CreateInstance(type);
            var asymblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asymblies)
            {
                var types = asm.GetTypes();
                foreach (var foundType in types)
                {
                    if (foundType.BaseType == null || (foundType.BaseType.FullName != "LTest.Core.Logic.LoadTest" ||
                                                       !foundType.FullName.Contains(strFullyQualifiedName))) continue;
                    //var assymbly= AppDomain.CurrentDomain.Load(File.ReadAllBytes(foundType.Assembly.Location));
                    //var file = new FileInfo(foundType.Assembly.Location);
                    return Activator.CreateInstance(foundType) as ILoadTest;
                }
            }
            return null;
        }
    }
}
using System;
using LTest.Core.Interfaces;
using LTest.Runner.Interfaces;

namespace LTest.Runner
{
    public class Worker : IWorker
    {
        private readonly ILoadTest _test;
        private readonly TestEndHandlerArgs _status;

        public Worker(string testType)
        {
            _test = GetInstance(testType);
            Name = _test.TestName;
            _status = new TestEndHandlerArgs();
            _status.TestName = _test.TestName;
        }

        public event TestEndHandler OnTestEnd;
        public string Name { get; set; }

        public void RunTest()
        {
            Before();
            Execute();
            After();
            InvokeOnTestEnd(_status);
        }

        private void After()
        {
            var timeStart = DateTime.Now;
            try
            {
                _test.AfterTest();
            }
            catch (Exception e)
            {
                _status.Execption = e;
                _status.IsSuccess = false;
            }

            var timeStop = DateTime.Now;
            _status.TimeForCleanup = timeStop - timeStart;
        }

        private void Execute()
        {
            var timeStart = DateTime.Now;
            try
            {
                _test.Execute();
            }
            catch (Exception e)
            {
                _status.Execption = e;
                _status.IsSuccess = false;
            }

            var timeStop = DateTime.Now;
            _status.TimeForTest = timeStop - timeStart;
        }

        private void Before()
        {
            var timeStart = DateTime.Now;
            try
            {
                _test.BeforeTest();
            }
            catch (Exception e)
            {
                _status.Execption = e;
                _status.IsSuccess = false;
            }

            var timeStop = DateTime.Now;
            _status.TimeForSetup = timeStop - timeStart;
        }

        private static ILoadTest GetInstance(string strFullyQualifiedName)
        {
            var type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return (ILoadTest) Activator.CreateInstance(type);
            var asymblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asymblies)
            {
                var types = asm.GetTypes();
                foreach (var foundType in types)
                {
                    if (foundType.BaseType == null || foundType.BaseType.FullName != "LTest.Core.Logic.LoadTest" ||
                        !foundType.FullName.Contains(strFullyQualifiedName)) continue;
                    return Activator.CreateInstance(foundType) as ILoadTest;
                }
            }

            return null;
        }

        protected virtual void InvokeOnTestEnd(TestEndHandlerArgs args)
        {
            OnTestEnd?.Invoke(this, args);
        }
    }
}
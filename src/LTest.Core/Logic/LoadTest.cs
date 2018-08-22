using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTest.Core.Interfaces;

namespace LTest.Core.Logic
{
    public abstract class LoadTest:ILoadTest
    {
        public virtual void BeforeTest()
        {
            Console.WriteLine("Before Test Execution!");
        }

        public virtual void AfterTest()
        {
            Console.WriteLine("After Test Execution!");
        }

        public abstract void Execute();

    }
}

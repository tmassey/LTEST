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
        public virtual string TestName { get; } = "BASE";

        public virtual void BeforeTest()
        {
            
        }

        public virtual void AfterTest()
        {
            
        }

        public abstract void Execute();

    }
}

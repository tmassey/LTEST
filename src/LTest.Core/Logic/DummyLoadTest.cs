using System;
using System.Threading;
using LTest.Core.Interfaces;

namespace LTest.Core.Logic
{
    public class DummyLoadTest : LoadTest
    {
       public override void BeforeTest()
        {            
            base.BeforeTest();
            Console.Write("B");
        }

        public override void Execute()
        {
            Console.Write("#");
            //Thread.Sleep(100);
        }

        public override void AfterTest()
        {            
            base.AfterTest();
            Console.Write("A");
        }
    }
}
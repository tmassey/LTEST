using System;

namespace LTest.Core.Logic
{
    public class DummyLoadTest : LoadTest
    {
        public override void BeforeTest()
        {
            base.BeforeTest();
            Console.WriteLine("Before Dummy Test!!!");
        }

        public override void Execute()
        {
            Console.WriteLine("Dummy Load Test Ran");
        }

        public override void AfterTest()
        {
            base.AfterTest();
            Console.WriteLine("After Dummy Test!!!");
        }
    }
}
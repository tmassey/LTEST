using System;
using System.Net;
using System.Threading;
using LTest.Core.Interfaces;
using RestSharp;

namespace LTest.Core.Logic
{
    public class DummyLoadTest : LoadTest
    {
        private Caller _caller;

        public override string TestName { get; } = "DUMMY TEST";

        public override void BeforeTest()
        {
            base.BeforeTest();
            _caller = new Caller(@"https://jsonplaceholder.typicode.com/");
        }

        public override void Execute()
        {
            var y = 0;
            var results = _caller.MakeCall<SampleReults>(Method.GET, "todos/1");
            //if(results.StatusCode==HttpStatusCode.OK)
            //var x = 1 / y;

        }

        public override void AfterTest()
        {
            base.AfterTest();
            _caller = null;
        }

        public class SampleReults
        {
            public int userId { get; set; }
            public int id { get; set; }
            public string title { get; set; }
            public bool completed { get; set; }
        }
    }
}
using System.Net;
using System.Net.Http;
using RestSharp;

namespace LTest.Core.Logic
{
    public class GetBaseAllChurchesLoadTest : LoadTest
    {
        private Caller _caller;

        public override string TestName { get; } = "base/churches/";

        public override void BeforeTest()
        {
            base.BeforeTest();
            _caller = new Caller(@"https://dapi.smchcn.net/api/v1.0/");
        }

        public override void Execute()
        {
            var y = 0;
            var results = _caller.MakeCall<dynamic>(Method.GET, "base/churches/53114");
            if (results.StatusCode != HttpStatusCode.OK && results.StatusCode != HttpStatusCode.NoContent)
                throw new HttpRequestException(TestName + " Failed!");
        }

        public override void AfterTest()
        {
            base.AfterTest();
            _caller = null;
        }
    }
}
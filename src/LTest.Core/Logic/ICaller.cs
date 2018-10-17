using System.Collections.Generic;
using RestSharp;

namespace LTest.Core.Logic
{
    public interface ICaller
    {
        void SetBaseUrl(string baseUrl);
        RequesterResponse<T> MakeCall<T>(Method method, string endpointUri, object data = null, IDictionary<string, string> headers = null) where T : new();
        IRestResponse MakeCall(Method method, string endpointUri, object data = null, IDictionary<string, string> headers = null);
    }
}
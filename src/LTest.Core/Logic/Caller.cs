using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace LTest.Core.Logic
{
    public class Caller: ICaller
    {
        private readonly IRestClient _client;

        public Caller()
        {
            _client = new RestClient();
        }



        public Caller(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public Caller(IRestClient restClient)
        {
            _client = restClient;
        }

        public void SetBaseUrl(string baseUrl)
        {
            _client.BaseUrl = new Uri(baseUrl);
        }

        public RequesterResponse<T> MakeCall<T>(Method method, string endpointUri, object data = null, IDictionary<string, string> headers = null) where T : new()
        {
            CheckForClient();
            var req = new RestRequest(endpointUri, method) { RequestFormat = DataFormat.Json }; ;
            AddHeaders(req, headers);
            AddBody(data, req);
            var resp = ExecuteRequest<T>(req);
            return CreateResponseFromRestResponse(resp);
        }


        private static RequesterResponse<T> CreateResponseFromRestResponse<T>(IRestResponse<T> resp) where T : new()
        {
            return new RequesterResponse<T>()
            {
                StatusCode = resp.StatusCode,
                StatusDescription = resp.StatusDescription,
                Data = resp.Data
            };
        }

        private IRestResponse<T> ExecuteRequest<T>(IRestRequest req) where T : new()
        {
            var resp = _client.Execute<T>(req);

            if (resp == null)
                throw new RequesterException(HttpStatusCode.InternalServerError,
                    "No response from request. Something really bad happened.");

            if (resp.ErrorException != null)
                throw new RequesterException(HttpStatusCode.InternalServerError, resp.ErrorMessage);
            return resp;
        }

        private static void AddBody(object data, IRestRequest req)
        {
            if (data != null) req.AddBody(data);
        }

        public IRestResponse MakeCall(Method method, string endpointUri, object data = null, IDictionary<string, string> headers = null)
        {
            CheckForClient();
            var req = new RestRequest(endpointUri, method) { RequestFormat = DataFormat.Json };
            AddHeaders(req, headers);
            AddBody(data, req);
            var resp = ExecuteUntypedRequest(req);
            return resp;
        }

        private void CheckForClient()
        {
            if (_client?.BaseUrl == null)
                throw new RequesterException(HttpStatusCode.ExpectationFailed,
                    "No rest client. You must create class with baseUrl, or call SetBaseUrl before calling MakeCall.");
        }

        private IRestResponse ExecuteUntypedRequest(IRestRequest req)
        {
            var resp = _client.Execute(req);

            if (resp == null)
                throw new RequesterException(HttpStatusCode.InternalServerError,
                    "No response from request. Something really bad happened.");

            return resp;
        }


        private void AddHeaders(IRestRequest req, IDictionary<string, string> headers)
        {
            if (headers == null) return;
            foreach (var header in headers)
            {
                req.AddHeader(header.Key, header.Value);
            }           
        }
    }
}
using System;
using System.Net;

namespace LTest.Core.Logic
{
    public class RequesterException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public RequesterException(HttpStatusCode statusCode, string message, Exception internalException = null) : base(message, internalException)
        {
            StatusCode = statusCode;
        }
    }
}
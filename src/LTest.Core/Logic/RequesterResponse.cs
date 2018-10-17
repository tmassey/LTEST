using System.Net;

namespace LTest.Core.Logic
{
    public class RequesterResponse<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }
}
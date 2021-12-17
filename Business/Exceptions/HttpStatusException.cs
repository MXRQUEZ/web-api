using System;
using System.Net;

namespace Business.Exceptions
{
    public sealed class HttpStatusException : Exception
    {
        public HttpStatusException(HttpStatusCode status, string msg) : base(msg)
        {
            Status = status;
        }

        public HttpStatusCode Status { get; }
    }
}
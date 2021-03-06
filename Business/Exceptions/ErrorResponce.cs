using System;

namespace Business.Exceptions
{
    public sealed class ErrorResponse
    {
        public ErrorResponse(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = ex.ToString();
        }

        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
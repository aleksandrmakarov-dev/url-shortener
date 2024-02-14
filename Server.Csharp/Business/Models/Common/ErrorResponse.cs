namespace Server.Csharp.Business.Models.Common
{
    public class ErrorResponse
    {
        public ErrorResponse() { }
        public ErrorResponse(int statusCode, string error, string message)
        {
            StatusCode = statusCode;
            Error = error;
            Message = message;
        }

        public int StatusCode { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }

    }
}

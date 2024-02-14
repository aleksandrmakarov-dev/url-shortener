using System.Net;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Presentation.Exceptions;

namespace Server.Csharp.Presentation.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HttpResponse response = httpContext.Response;
                response.ContentType = "application/json";

                ErrorResponse errorResponse = new ErrorResponse();

                switch (ex)
                {
                    case UnauthorizedException:
                        errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorResponse.Error = "401 Unauthorized";
                        break;
                    case NotFoundException:
                        errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse.Error = "404 NotFound";
                        break;
                    case ConflictException:
                        errorResponse.StatusCode = (int)HttpStatusCode.Conflict;
                        errorResponse.Error = "409 Conflict";
                        break;
                    default:
                        errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.Error = "501 Internal Server Error";
                        break;
                }

                
                errorResponse.Message = ex.Message;

                response.StatusCode = errorResponse.StatusCode;

                await response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}

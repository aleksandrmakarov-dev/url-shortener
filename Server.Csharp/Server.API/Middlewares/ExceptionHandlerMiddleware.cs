using System.Net;
using Server.Infrastructure.Exceptions;
using Server.Infrastructure.Models.Responses;

namespace Server.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse
                {
                    StatusCode = 0,
                    Message = "",
                    Error = ""
                };

                switch (ex)
                {
                    case BadRequestException:
                        errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.Error = "Bad Request";
                        break;
                    case UnauthorizedException:
                        errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorResponse.Error = "Unauthorized";
                        break;
                    case ForbiddenException:
                        errorResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.Error = "Forbidden";
                        break;
                    case NotFoundException:
                        errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse.Error = "Not Found";
                        break;
                    case UnprocessableEntityException:
                        errorResponse.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        errorResponse.Error = "Unprocessable Entity";
                        break;
                    default:
                        errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.Error = "Internal Server Error";
                        break;
                }
                errorResponse.Message = ex.Message;

                HttpResponse response = context.Response;

                response.ContentType = "application/json";
                response.StatusCode = errorResponse.StatusCode;

                await response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}

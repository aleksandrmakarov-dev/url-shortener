using Server.API.Common;
using Server.Infrastructure.Models;
using Server.Infrastructure.Services;

namespace Server.API.Middlewares
{
    public class UserExtractorMiddleware
    {
        private readonly RequestDelegate _next;

        public UserExtractorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            // get access token from authorization header
            string? accessToken = context.Request.Headers.Authorization
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            //if access token is null, go to next
            if (accessToken != null)
            {
                try
                {
                    // try to validate token
                    JwtPayload? payload = jwtService.ValidateToken(accessToken);
                    // set payload to context
                    context.Items[Constants.UserContextName] = payload;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            await _next(context);
        }
    }
}

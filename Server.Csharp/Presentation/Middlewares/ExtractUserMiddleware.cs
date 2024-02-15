using Server.Csharp.Business.Common;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Business.Services;

namespace Server.Csharp.Presentation.Middlewares
{
    public class ExtractUserMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ITokenService _tokenService;

        public ExtractUserMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? jwtToken = context.Request.Headers.Authorization
                .FirstOrDefault()?
                .Split(' ')
                .LastOrDefault();

            if (jwtToken == null)
            {
                await _next(context);
            }
            else
            {
                try
                {
                    JwtTokenPayload payload = _tokenService.ValidateJwtToken(jwtToken);
                    context.Items[Constants.User] = payload;
                }
                finally
                {
                    await _next(context);
                }
            }
        }
    }
}

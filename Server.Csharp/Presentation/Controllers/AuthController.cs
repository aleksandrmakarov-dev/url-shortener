using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Csharp.Business.Common;
using Server.Csharp.Business.Models;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Business.Models.Responses;
using Server.Csharp.Business.Services;
using Server.Csharp.Presentation.Attributes;
using Server.Csharp.Presentation.Common;
using Server.Csharp.Presentation.Exceptions;

namespace Server.Csharp.Presentation.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [RoleAuthorize(Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return Ok(new { message = "Hello!" });
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            EmailVerificationToken emailVerificationToken = await _authService.SignUpAsync(request);

            await _emailService.SendAsync(new SendEmailOptions
            {
                To = emailVerificationToken.Email,
                From = "noreply@example.com",
                Subject = "Verify your Account",
                Body = $"Your verification token is {emailVerificationToken.Value}"
            });

            return Ok();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            TokenResponse tokens = await _authService.SignInAsync(request);

            // Adding cookie to the response
            HttpContext.Response.Cookies.Append(
                Constants.RefreshTokenCookie,
                tokens.RefreshToken,
                Constants.CookieOptions(DateTime.UtcNow.AddMinutes(Constants.RefreshTokenExpires)));

            SessionResponse response = new SessionResponse()
            {
                AccessToken = tokens.AccessToken,
                UserId = tokens.UserId 
            };

            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            await _authService.VerifyEmailAsync(request);

            return Ok(new MessageResponse("Email verification","Email has been successfully verified."));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            string? refreshToken = HttpContext.Request.Cookies[Constants.RefreshTokenCookie];

            if (refreshToken == null)
            {
                throw new BadRequestException("Refresh token not provided.");
            }

            SessionResponse response = await _authService.RefreshTokenAsync(new RefreshTokenRequest
            {
                Token = refreshToken
            });

            return Ok(response);
        }

        [HttpDelete("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            HttpContext.Response.Cookies.Delete(Constants.RefreshTokenCookie,Constants.CookieOptions());

            return NoContent();
        }
    }
}

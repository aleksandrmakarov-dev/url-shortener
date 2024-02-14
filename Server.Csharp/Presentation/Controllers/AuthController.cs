using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Csharp.Business.Models;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Business.Models.Responses;
using Server.Csharp.Business.Services;

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
            HttpContext.Response.Cookies.Append("refresh-token",tokens.RefreshToken,new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            SignInResponse response = new SignInResponse
            {
                AccessToken = tokens.AccessToken
            };

            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            await _authService.VerifyEmailAsync(request);

            return Ok(new MessageResponse("Email verification","Email has been successfully verified."));
        }
    }
}

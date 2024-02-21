using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.Common;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;
using Server.Infrastructure.Services;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            EmailVerificationResponse emailVerification = await _authService.SignUpAsync(request);

            // send verification token
            Console.WriteLine(emailVerification.EmailVerificationToken);

            // message that user is created and needs to be verified
            MessageResponse response =  new MessageResponse
            {
                Title = "Complete the registration",
                Message = "You will receive verification code to the email address"
            };

            return Ok(response);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            // try sign in with email and password
            SignInResponse signIn = await _authService.SignInAsync(request);

            // set refresh token cookie to response
            HttpContext.Response.Cookies.Append(
                CookieNameConstants.RefreshToken,
                signIn.RefreshToken,
                Constants.CookieOptions(DateTime.UtcNow.AddDays(7))
            );

            // return session
            return Ok(signIn.Session);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            // try to verify email
            await _authService.VerifyEmailAsync(request);

            // message that email is verified
            MessageResponse response = new MessageResponse
            {
                Title = "Registration is completed",
                Message = "Your email address is verified. You can sign in to your account"
            };

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // get refresh token from cookie
            string? refreshToken = HttpContext.Request.Cookies[CookieNameConstants.RefreshToken];

            // if refreshToken cookie not found throw an error
            if (refreshToken == null)
            {
                throw new Exception("No refresh token provided");
            }
            
            // try to get session with refresh token
            SessionResponse session = await _authService.RefreshTokenAsync(new RefreshTokenRequest
            {
                Token = refreshToken
            });

            return Ok(session);
        }

        [HttpPost("new-email-verification")]
        public async Task<IActionResult> NewEmailVerification(EmailVerificationRequest request)
        {
            EmailVerificationResponse? emailVerification = await _authService.NewEmailVerificationAsync(request);

            if (emailVerification == null)
            {
                MessageResponse alreadyVerified = new MessageResponse
                {
                    Title = "Email verification",
                    Message = "Email is already verified. You can sign in to your account"
                };

                return Ok(alreadyVerified);
            }

            // send email verification

            Console.WriteLine(emailVerification.EmailVerificationToken);

            MessageResponse response = new MessageResponse
            {
                Title = "New email verification",
                Message = "New email verification token is sent to your email address"
            };

            return Ok(response);
        }
    }
}

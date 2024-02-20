using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<EmailVerificationResponse> SignUpAsync(SignUpRequest request);
        Task<SignInResponse> SignInAsync(SignInRequest request);
        Task VerifyEmailAsync(VerifyEmailRequest request);
        Task<SessionResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<EmailVerificationResponse?> NewEmailVerificationAsync(EmailVerificationRequest request);
    }
}

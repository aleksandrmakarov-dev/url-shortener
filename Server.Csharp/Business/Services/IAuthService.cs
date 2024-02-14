using Server.Csharp.Business.Models.Common;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Business.Models.Responses;

namespace Server.Csharp.Business.Services
{
    public interface IAuthService
    {
        Task<EmailVerificationToken> SignUpAsync(SignUpRequest request);
        Task<TokenResponse> SignInAsync(SignInRequest request);
        Task VerifyEmailAsync(VerifyEmailRequest request);
    }
}

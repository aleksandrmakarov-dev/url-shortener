using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<SignUpResponse> SignUpAsync(SignUpRequest request);
        Task<SignInResponse> SignInAsync(SignInRequest request);
    }
}

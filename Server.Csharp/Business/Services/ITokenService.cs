using Server.Csharp.Business.Models.Common;
using Server.Csharp.Data.Models;

namespace Server.Csharp.Business.Services
{
    public interface ITokenService
    {
        string GetToken(int length);
        string GetJwtToken(User user);
        JwtTokenPayload ValidateJwtToken(string token);
    }
}

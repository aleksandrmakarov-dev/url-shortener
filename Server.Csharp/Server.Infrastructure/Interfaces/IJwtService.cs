using JwtPayload = Server.Infrastructure.Models.JwtPayload;

namespace Server.Infrastructure.Interfaces
{
    public interface IJwtService
    {
        string GetToken(JwtPayload payload);
        JwtPayload? ValidateToken(string token);
    }
}

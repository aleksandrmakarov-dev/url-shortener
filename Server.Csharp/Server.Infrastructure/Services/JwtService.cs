using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Infrastructure.Common;
using Server.Infrastructure.Models;
using JwtPayload = Server.Infrastructure.Models.JwtPayload;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Server.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly string _jwtKey = "my-secure-jwt-encoding-key";
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtService()
    {
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public string GetToken(JwtPayload payload)
    {
        // generate symmetric security key
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

        // create signing credentials
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtNameConstants.JwtId,payload.Id.ToString()),
            new Claim(JwtNameConstants.JwtRole,payload.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        JwtSecurityToken token = new JwtSecurityToken(
            "",
            "",
            claims,
            expires:DateTime.UtcNow.AddMinutes(15),
            signingCredentials:credentials
        );

        return _jwtSecurityTokenHandler.WriteToken(token);
    }

    public JwtPayload? ValidateToken(string token)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters,
                out SecurityToken validatedToken);

            JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)validatedToken;

            JwtPayload payload = new JwtPayload
            {
                Id = Guid.Parse(jwtSecurityToken.Claims.First(c => c.Type == JwtNameConstants.JwtId).Value),
                Role = Enum.Parse<Role>(
                    jwtSecurityToken.Claims.First(c => c.Type == JwtNameConstants.JwtRole).Value)
            };

            return payload;
        }
        catch
        {
            return null;
        }

    }
}
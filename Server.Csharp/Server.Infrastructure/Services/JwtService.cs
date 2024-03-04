using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Infrastructure.Common;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models;
using Server.Infrastructure.Options;

namespace Server.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JsonWebTokenOptions _options;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtService(IOptions<JsonWebTokenOptions> options)
    {
        _options = options.Value;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public string GetToken(Models.JwtPayload payload)
    {
        // generate symmetric security key
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

        // create signing credentials
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

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

    public Models.JwtPayload? ValidateToken(string token)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

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

            Models.JwtPayload payload = new Models.JwtPayload
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
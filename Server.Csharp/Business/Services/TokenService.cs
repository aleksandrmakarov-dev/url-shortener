using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Data.Models;

namespace Server.Csharp.Business.Services;

public class TokenService : ITokenService
{
    private readonly string _secretKey = "this-is-very-secret-key-to-encode-jwt";
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public TokenService()
    {
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public string GetToken(int length)
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(length));
    }

    public string GetJwtToken(User user)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        string securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
        SigningCredentials signingCredentials = new SigningCredentials(
            securityKey,
            securityAlgorithm
        );

        JwtPayload jwtPayload = new JwtPayload(new[]
        {
            new Claim("id", user.Id.ToString())
        });

        JwtHeader jwtHeader = new JwtHeader(signingCredentials);
        JwtSecurityToken jwtToken = new JwtSecurityToken(jwtHeader,jwtPayload);

        return _jwtSecurityTokenHandler.WriteToken(jwtToken);
    }

    public JwtTokenPayload ValidateJwtToken(string token)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        _jwtSecurityTokenHandler.ValidateToken(
            token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            },
            out SecurityToken validatedToken
        );

        JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

        JwtTokenPayload payload = new JwtTokenPayload
        {
            Id = Guid.Parse(jwtToken.Claims.First(c=>c.Type == "id").Value)
        };

        return payload;
    }
}
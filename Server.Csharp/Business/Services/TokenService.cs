using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Csharp.Business.Common;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Business.Options;
using Server.Csharp.Data.Entities;
using Server.Csharp.Migrations;
using Server.Csharp.Presentation.Common;

namespace Server.Csharp.Business.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public string GetToken(int length)
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(length));
    }

    public string GetJwtToken(User user)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        string securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
        SigningCredentials signingCredentials = new SigningCredentials(
            securityKey,
            securityAlgorithm
        );

        JwtPayload jwtPayload = new JwtPayload(
            _jwtOptions.Issuer,
            _jwtOptions.Issuer,
            new []
            {
                new Claim(Constants.IdClaim,user.Id.ToString()),
                new Claim(Constants.RoleClaim,user.Role.Name)
            },
            null,
            DateTime.UtcNow.AddMinutes(Constants.JwtTokenExpires),
            DateTime.UtcNow
            );

        JwtHeader jwtHeader = new JwtHeader(signingCredentials);
        JwtSecurityToken jwtToken = new JwtSecurityToken(jwtHeader,jwtPayload);

        return _jwtSecurityTokenHandler.WriteToken(jwtToken);
    }

    public JwtTokenPayload ValidateJwtToken(string token)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
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
            Id = Guid.Parse(jwtToken.Claims.First(c=>c.Type == Constants.IdClaim).Value),
            Role = Enum.Parse<Roles>(jwtToken.Claims.First(c => c.Type == Constants.RoleClaim).Value)
        };

        return payload;
    }
}
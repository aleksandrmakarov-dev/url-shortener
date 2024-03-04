using System.Security.Cryptography;
using Server.Infrastructure.Interfaces;

namespace Server.Infrastructure.Services;

public class TokensService : ITokensService
{
    public string GetToken(int count)
    {
        return RandomNumberGenerator.GetHexString(count, true);
    }
}
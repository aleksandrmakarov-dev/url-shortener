using Server.Infrastructure.Interfaces;

namespace Server.Infrastructure.Services;

public class BcryptPasswordsService : IPasswordsService
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, 13);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
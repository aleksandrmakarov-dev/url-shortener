

namespace Server.Infrastructure.Services
{
    public interface IPasswordsService
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}

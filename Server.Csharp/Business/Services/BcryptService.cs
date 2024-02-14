using BCrypt.Net;

namespace Server.Csharp.Business.Services
{
    public class BcryptService:IPasswordService
    {

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}

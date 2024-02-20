using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Services
{
    public interface ITokensService
    {
        public string GetToken(int count);
    }

    public class TokensService : ITokensService
    {
        public string GetToken(int count)
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(count));
        }
    }
}

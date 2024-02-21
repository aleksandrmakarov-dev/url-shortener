using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Services
{
    public interface ITokensService
    {
        public string GetToken(int count);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Responses
{
    public class SignInResponse
    {
        public required string RefreshToken { get; set; }
        public required SessionResponse Session { get; set; }
    }
}

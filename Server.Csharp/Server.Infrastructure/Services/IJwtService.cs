using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Infrastructure.Models.Requests;
using JwtPayload = Server.Infrastructure.Models.JwtPayload;

namespace Server.Infrastructure.Services
{
    public interface IJwtService
    {
        string GetToken(JwtPayload payload);
        JwtPayload? ValidateToken(string token);
    }
}

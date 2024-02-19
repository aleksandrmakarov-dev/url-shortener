using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Database;
using Server.Data.Entities;

namespace Server.Data.Repositories
{
    public class SessionsRepository(ApplicationDbContext context) :GenericRepository<Session>(context), ISessionsRepository
    {
        public async Task<Session?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await context.Sessions.FirstOrDefaultAsync(s => s.RefreshToken == refreshToken);
        }
    }
}

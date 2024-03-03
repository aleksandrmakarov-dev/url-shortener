using Microsoft.EntityFrameworkCore;
using Server.Data.Database;
using Server.Data.Entities;

namespace Server.Data.Repositories
{
    public class SessionsRepository(ApplicationDbContext context) :GenericRepository<Session>(context), ISessionsRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Session?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Sessions.FirstOrDefaultAsync(s => s.RefreshToken == refreshToken);
        }
    }
}

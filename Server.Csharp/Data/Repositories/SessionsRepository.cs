using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Csharp.Data.Database;
using Server.Csharp.Data.Entities;

namespace Server.Csharp.Data.Repositories
{
    public class SessionsRepository:GenericRepository<Session>,ISessionsRepository
    {
        public SessionsRepository(ApplicationDbContext context, IMapper mapper) : base(context,mapper)
        {
        }

        public async Task<Session?> GetByRefreshTokenAsync(string token)
        {
            return await Context.Sessions.FirstOrDefaultAsync(s => s.RefreshToken == token);
        }
    }
}

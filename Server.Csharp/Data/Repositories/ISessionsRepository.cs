using Server.Csharp.Data.Entities;

namespace Server.Csharp.Data.Repositories;

public interface ISessionsRepository : IGenericRepository<Session>
{
    Task<Session?> GetByRefreshTokenAsync(string token);
}
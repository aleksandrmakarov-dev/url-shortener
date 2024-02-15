using Server.Csharp.Data.Entities;

namespace Server.Csharp.Business.Services;

public interface ISessionsService
{
    Task<Session> CreateAsync(Guid userId);
}
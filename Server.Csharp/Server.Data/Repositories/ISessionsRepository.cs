using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Data.Entities;

namespace Server.Data.Repositories
{
    public interface ISessionsRepository:IGenericRepository<Session>
    {
        Task<Session?> GetByRefreshTokenAsync(string refreshToken);
    }
}

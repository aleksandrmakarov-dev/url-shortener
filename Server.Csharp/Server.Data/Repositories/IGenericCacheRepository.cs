using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Data.Entities;

namespace Server.Data.Repositories
{
    public interface IGenericCacheRepository<TEntity> where TEntity:Entity
    {
        Task<TEntity?> GetAsync(string key);
        Task SetAsync(string key,TEntity entity, TimeSpan? ttl = null);
        Task DeleteAsync(string key);
        Task RefreshAsync(string key);
    }
}

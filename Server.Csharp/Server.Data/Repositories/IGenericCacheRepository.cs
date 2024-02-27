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
        Task<TEntity?> GetByKeyAsync(string key);
        Task SetAsync(string key,TEntity entity);
        Task DeleteByKeyAsync(string key);
    }
}

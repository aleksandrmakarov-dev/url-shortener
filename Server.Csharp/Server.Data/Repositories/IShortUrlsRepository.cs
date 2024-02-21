using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Data.Entities;

namespace Server.Data.Repositories
{
    public interface IShortUrlsRepository:IGenericRepository<ShortUrl>
    {
        Task<ShortUrl?> GetByAliasAsync(string alias);

        Task<bool> IsExistByAliasAsync(string alias);
    }
}

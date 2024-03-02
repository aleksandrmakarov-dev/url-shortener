using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Server.Data.Entities;

namespace Server.Data.Repositories
{
    public interface INavigationsRepository:IGenericRepository<Navigation>
    {
        public Task<IEnumerable<KeyValuePair<TKey, int>>> CountByShortUrlIdAndGroupAsync<TKey>(Guid shortUrlId,
            Expression<Func<Navigation, TKey>> groupExpression);
    }
}

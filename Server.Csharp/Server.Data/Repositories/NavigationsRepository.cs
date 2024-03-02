using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Server.Data.Database;
using Server.Data.Entities;

namespace Server.Data.Repositories;

public class NavigationsRepository(ApplicationDbContext context) : GenericRepository<Navigation>(context), INavigationsRepository
{
    public async Task<IEnumerable<KeyValuePair<TKey, int>>> CountByShortUrlIdAndGroupAsync<TKey>(Guid shortUrlId, Expression<Func<Navigation, TKey>> groupExpression)
    {
        return await context.Navigations
            .Where(n => n.ShortUrlId == shortUrlId)
            .GroupBy(groupExpression)
            .Select(n => new KeyValuePair<TKey,int>(n.Key,n.Count())).ToListAsync();
    }
}
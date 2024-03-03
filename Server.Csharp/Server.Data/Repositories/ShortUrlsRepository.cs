using Microsoft.EntityFrameworkCore;
using Server.Data.Database;
using Server.Data.Entities;

namespace Server.Data.Repositories;

public class ShortUrlsRepository(ApplicationDbContext context) : GenericRepository<ShortUrl>(context), IShortUrlsRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ShortUrl?> GetByAliasAsync(string alias)
    {
        return await _context.ShortUrls.FirstOrDefaultAsync(su => su.Alias == alias);
    }

    public async Task<bool> IsExistByAliasAsync(string alias)
    {
        return await _context.ShortUrls.AnyAsync(su=>su.Alias == alias);
    }
}
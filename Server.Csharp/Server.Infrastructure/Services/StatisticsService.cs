using Microsoft.EntityFrameworkCore;
using Server.Data.Database;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services;

public class StatisticsService:IStatisticsService
{
    ApplicationDbContext _context;

    public StatisticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StatisticsResponse> GetByShortUrlId(Guid id)
    {
        int navigationCount = await _context.Navigations.CountAsync(n=>n.ShortUrlId == id);

        List<StatisticsItem> countryAndClicksList = await _context.Navigations
            .Where(n=> n.ShortUrlId == id)
            .GroupBy(n => n.CountryName)
            .Select(n=>new StatisticsItem
            {
                Name = n.Key,
                Count = n.Count()
            }).ToListAsync();

        List<StatisticsItem> platformAndClicksList = await _context.Navigations
            .Where(n=>n.ShortUrlId == id)
            .GroupBy(n=>n.Platform)
            .Select(n => new StatisticsItem
            {
                Name = n.Key,
                Count = n.Count()
            }).ToListAsync();

        List<StatisticsItem> browserAndClicksList = await _context.Navigations
            .Where(n => n.ShortUrlId == id)
            .GroupBy(n => n.Browser)
            .Select(n => new StatisticsItem
            {
                Name = n.Key,
                Count = n.Count()
            }).ToListAsync();

        StatisticsResponse response = new StatisticsResponse
        {
            NavigationCount = navigationCount,
            Countries = countryAndClicksList,
            Platforms = platformAndClicksList,
            Browsers = browserAndClicksList
        };

        return response;
    }
}
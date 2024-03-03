using Server.Data.Repositories;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services;

public class StatisticsService:IStatisticsService
{
    private readonly INavigationsRepository _navigationRepository;

    public StatisticsService(INavigationsRepository navigationRepository)
    {
        _navigationRepository = navigationRepository;
    }

    public async Task<StatisticsResponse> GetByShortUrlId(Guid id)
    {


        // calculate total number of navigation
        int navigationCount = await _navigationRepository.CountAsync(n => n.ShortUrlId == id);

        // calculate number of navigation from different countries
        IEnumerable<KeyValuePair<string, int>> countryAndClicksList =
            await _navigationRepository.CountByShortUrlIdAndGroupAsync(id, n => n.Country);

        // calculate number of navigation from different platforms
        IEnumerable<KeyValuePair<string, int>> platformAndClicksList =
            await _navigationRepository.CountByShortUrlIdAndGroupAsync(id, n => n.Platform);

        // calculate number of navigation from different browsers
        IEnumerable<KeyValuePair<string, int>> browserAndClicksList =
            await _navigationRepository.CountByShortUrlIdAndGroupAsync(id, n => n.Browser);

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
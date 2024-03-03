using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatisticsResponse> GetByShortUrlId(Guid id);
    }
}

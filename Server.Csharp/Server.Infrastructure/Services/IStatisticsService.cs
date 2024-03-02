using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsResponse> GetByShortUrlId(Guid id);
    }
}

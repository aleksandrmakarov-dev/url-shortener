using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure.Exceptions;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models.Responses;

namespace Server.API.Controllers
{
    // [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {

        private readonly IStatisticsService _statisticsService;
        private readonly IShortUrlsService _shortUrlsService;

        public StatisticsController(IStatisticsService statisticsService, IShortUrlsService shortUrlsService)
        {
            _statisticsService = statisticsService;
            _shortUrlsService = shortUrlsService;

        }

        [HttpGet("{shortUrlId:guid}")]

        public async Task<IActionResult> GetByShorUrlId([FromRoute] Guid shortUrlId)
        {
            ShortUrlResponse? foundShortUrlResponse = await _shortUrlsService.GetByIdAsync(shortUrlId);

            if (foundShortUrlResponse == null)
            {
                throw new NotFoundException($"Short Url ${shortUrlId} not found");
            }

            StatisticsResponse statisticsResponse = await _statisticsService.GetByShortUrlId(foundShortUrlResponse.Id);

            return Ok(statisticsResponse);
        }
    }
}

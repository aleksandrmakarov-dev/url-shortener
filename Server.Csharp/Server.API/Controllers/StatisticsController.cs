using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure.Models.Responses;
using Server.Infrastructure.Services;

namespace Server.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("{shortUrlId:guid}")]

        public async Task<IActionResult> GetByShorUrlId([FromRoute] Guid shortUrlId)
        {
            StatisticsResponse statisticsResponse = await _statisticsService.GetByShortUrlId(shortUrlId);

            return Ok(statisticsResponse);
        }
    }
}

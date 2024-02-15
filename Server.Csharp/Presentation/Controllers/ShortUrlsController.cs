using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Csharp.Business.Common;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Business.Models.Responses;
using Server.Csharp.Business.Services;
using Server.Csharp.Data.Entities;
using Server.Csharp.Presentation.Common;
using Server.Csharp.Presentation.Exceptions;

namespace Server.Csharp.Presentation.Controllers
{
    [Route("api/v1/short-urls")]
    [ApiController]
    public class ShortUrlsController : ControllerBase
    {
        private readonly IShortUrlsService _shortUrlsService;

        public ShortUrlsController(IShortUrlsService shortUrlsService)
        {
            _shortUrlsService = shortUrlsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortUrl(CreateShortUrlRequest request)
        {
            JwtTokenPayload? user = (JwtTokenPayload?)HttpContext.Items[Constants.User];

            if (request.UserId != null && request.UserId != user?.Id && user?.Role != Roles.Admin)
            {
                throw new BadRequestException("Only admin users are allowed to create url for another user");
            }

            ShortUrlResponse shortUrlResponse = await _shortUrlsService.CreateAsync(request);

            return Ok(shortUrlResponse);
        }

        [HttpGet("redirect/{id:guid}")]
        public async Task<IActionResult> RedirectToUrl([FromRoute] Guid id)
        {
            RedirectResponse redirectResponse = await _shortUrlsService.GetRedirectUrlAsync(id);

            return Ok(redirectResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetShortUrls()
        {
            IEnumerable<ShortUrlResponse> foundShortUrls = await _shortUrlsService.GetAllAsync();

            return Ok(foundShortUrls);
        }
    }
}

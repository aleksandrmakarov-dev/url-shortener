using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.Common;
using Server.Infrastructure.Exceptions;
using Server.Infrastructure.Models;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;
using Server.Infrastructure.Services;

namespace Server.API.Controllers
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
        public async Task<IActionResult> Create([FromBody] CreateShortUrlRequest request)
        {
            // get extracted from access token user
            JwtPayload? user = (JwtPayload?)HttpContext.Items[Constants.UserContextName];

            // check if custom alias is set and if user is anonymous throw an error
            if (request.CustomAlias != null && user == null)
            {
                throw new UnauthorizedException("Anonymous users can't create short url with custom alias");
            }

            // probably works but requires testing...
            // if user id received from token not equal to user id in body and user role is not an admin, throw an error
            if (request.UserId != user?.Id && user?.Role != Role.Admin)
            {
                throw new UnauthorizedException("Only admin users can create short-url with other user id");
            }

            // create short url
            ShortUrlResponse shortUrlResponse = await _shortUrlsService.CreateAsync(request);

            // return short url response
            return Ok(shortUrlResponse);
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> GetByAlias([FromRoute] string alias)
        {
            ShortUrlResponse shortUrlResponse = await _shortUrlsService.GetByAliasAsync(alias);

            return Ok(shortUrlResponse);
        }
    }
}

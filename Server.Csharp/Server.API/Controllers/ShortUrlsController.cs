﻿using Microsoft.AspNetCore.Mvc;
using Server.API.Attributes;
using Server.API.Common;
using Server.Infrastructure.Exceptions;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models;
using Server.Infrastructure.Models.Filters;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.API.Controllers
{
    [Authorize]
    [Route("api/v1/short-urls")]
    [ApiController]
    public class ShortUrlsController : ControllerBase
    {
        private readonly IShortUrlsService _shortUrlsService;
        private readonly INavigationsService _navigationService;
        private readonly ILocationService _locationService;

        public ShortUrlsController(IShortUrlsService shortUrlsService, INavigationsService navigationService, ILocationService locationService)
        {
            _shortUrlsService = shortUrlsService;
            _navigationService = navigationService;
            _locationService = locationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShortUrlRequest request)
        {
            // get extracted from access token user
            JwtPayload? user = (JwtPayload?)HttpContext.Items[Constants.UserContextName];

            // check if custom alias is set and if user is anonymous throw an error

            if ((!string.IsNullOrEmpty(request.CustomAlias) || request.ExpiresAt != null) && user == null)
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

        [AllowAnonymous]
        [HttpGet("a/{alias}")]
        public async Task<IActionResult> GetByAlias([FromRoute] string alias, [FromQuery] bool? throwOnExpire)
        {
            ShortUrlResponse shortUrlResponse = await _shortUrlsService.GetByAliasAsync(new GetShortUrlByAliasRequest
            {
                Alias = alias,
                ThrowOnExpire = throwOnExpire
            });

            string? ipAddress = Helpers.GetIpAddress(HttpContext);

            CreateNavigationRequest createNavigationRequest = new CreateNavigationRequest
            {
                IpAddress = ipAddress,
                Country = "Unknown",
                Browser = "Unknown",
                Platform = "Unknown",
                ShortUrlId = shortUrlResponse.Id
            };

            if (ipAddress != null)
            {
                LocationResponse? locationResponse = await _locationService.GetByIpAddressAsync(ipAddress);

                if (locationResponse != null && locationResponse.Status != "failed")
                {
                    createNavigationRequest.Country = locationResponse.Country ?? "Unknown";
                }
            }

            await _navigationService.CreateAsync(createNavigationRequest);

            return Ok(shortUrlResponse);
        }

        [HttpGet("id/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            ShortUrlResponse shortUrlResponse = await _shortUrlsService.GetByIdAsync(id);

            return Ok(shortUrlResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid? userId,
            [FromQuery] string? query,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10
        )
        {
            JwtPayload? user = (JwtPayload?)HttpContext.Items[Constants.UserContextName];

            if (user == null || (user.Id != userId && user.Role != Role.Admin))
            {
                throw new UnauthorizedException("Only admin user can view other user's short URLs.");       
            }

            Paged<ShortUrlResponse> pageResponse = await _shortUrlsService.GetPageAsync(page, size, new ShortUrlsPageFilter
            {
                UserId = userId,
                Query = query,
            });
            return Ok(pageResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateById([FromRoute] Guid id, [FromBody] UpdateShortUrlRequest request)
        {
            //check that user can update only his short urls

            // get extracted from access token user
            JwtPayload? user = (JwtPayload?)HttpContext.Items[Constants.UserContextName];

            // get short url by id
            ShortUrlResponse foundShortUrlResponse = await _shortUrlsService.GetByIdAsync(id);

            // check if user tries to update anonymous short url

            if (foundShortUrlResponse.UserId == null && user?.Role != Role.Admin)
            {
                throw new UnauthorizedException("Only admin users can update anonymous short urls");
            }

            // check if current user id equal to user id who create short url or role is admin
            if (foundShortUrlResponse.UserId != user?.Id && user?.Role != Role.Admin)
            {
                throw new UnauthorizedException("User can't update other user's short url");
            }

            await _shortUrlsService.UpdateByIdAsync(id, request);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            //check that user can delete only his short urls

            // get extracted from access token user
            JwtPayload? user = (JwtPayload?)HttpContext.Items[Constants.UserContextName];

            // get short url by id
            ShortUrlResponse foundShortUrlResponse = await _shortUrlsService.GetByIdAsync(id);

            // check if user tries to delete anonymous short url

            if (foundShortUrlResponse.UserId == null && user?.Role != Role.Admin)
            {
                throw new UnauthorizedException("Only admin users can delete anonymous short urls");
            }

            // check if current user id equal to user id who create short url or role is admin
            if (foundShortUrlResponse.UserId != user?.Id && user?.Role != Role.Admin)
            {
                // throw an error
                throw new UnauthorizedException("User can't update other user's short url");
            }

            await _shortUrlsService.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}

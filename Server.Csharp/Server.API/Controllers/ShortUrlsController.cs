﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.Attributes;
using Server.API.Common;
using Server.Data.Entities;
using Server.Infrastructure.Exceptions;
using Server.Infrastructure.Models;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;
using Server.Infrastructure.Services;

namespace Server.API.Controllers
{
    [Authorize]
    [Route("api/v1/short-urls")]
    [ApiController]
    public class ShortUrlsController : ControllerBase
    {
        private readonly IShortUrlsService _shortUrlsService;

        public ShortUrlsController(IShortUrlsService shortUrlsService)
        {
            _shortUrlsService = shortUrlsService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("{alias}")]
        public async Task<IActionResult> GetByAlias([FromRoute] string alias)
        {
            ShortUrlResponse shortUrlResponse = await _shortUrlsService.GetByAliasAsync(alias);

            return Ok(shortUrlResponse);
        }

        // [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ShortUrlResponse> foundShortUrls = await _shortUrlsService.GetAllAsync();
            return Ok(foundShortUrls);
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
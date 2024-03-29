﻿using Microsoft.AspNetCore.Mvc;
using Server.API.Attributes;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models;
using Server.Infrastructure.Models.Responses;

namespace Server.API.Controllers
{
    [Authorize(Role.Admin)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UserResponse> foundUsers = await _usersService.GetAllAsync();

            return Ok(foundUsers);
        }
    }
}

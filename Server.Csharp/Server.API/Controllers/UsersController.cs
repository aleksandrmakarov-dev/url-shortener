using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure.Models.Responses;
using Server.Infrastructure.Services;

namespace Server.API.Controllers
{
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

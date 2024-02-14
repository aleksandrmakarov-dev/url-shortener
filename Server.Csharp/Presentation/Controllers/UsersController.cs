using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Csharp.Business.Models.Responses;
using Server.Csharp.Business.Services;
using Server.Csharp.Presentation.Exceptions;

namespace Server.Csharp.Presentation.Controllers
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            UserResponse? foundUser = await _usersService.GetByIdAsync<UserResponse>(id);

            if (foundUser == null)
            {
                throw new NotFoundException($"User id = ${id.ToString()} not found");
            }

            return Ok(foundUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            IEnumerable<UserResponse> foundUsers = await _usersService.GetAllAsync<UserResponse>();
            return Ok(foundUsers);
        }
    }
}

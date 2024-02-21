using AutoMapper;
using Server.Data.Entities;
using Server.Data.Repositories;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public UsersService(IMapper mapper, IUsersRepository usersRepository)
    {
        _mapper = mapper;
        _usersRepository = usersRepository;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        IEnumerable<User> foundUsers = await _usersRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<UserResponse>>(foundUsers);
    }
}
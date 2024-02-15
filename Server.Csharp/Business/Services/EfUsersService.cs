using AutoMapper;
using Server.Csharp.Business.Models.Responses;
using Server.Csharp.Data.Entities;
using Server.Csharp.Data.Repositories;

namespace Server.Csharp.Business.Services;

public class EfUsersService:IUsersService
{
    private readonly IMapper _mapper;
    private readonly IUsersRepository _usersRepository;

    public EfUsersService(IMapper mapper, IUsersRepository usersRepository)
    {
        _mapper = mapper;
        _usersRepository = usersRepository;
    }

    public async Task<TModel?> GetByIdAsync<TModel>(Guid id)
    {
        User? foundUser = await _usersRepository.GetByIdAsync(id);

        if (foundUser != null) return _mapper.Map<User, TModel>(foundUser);

        return default(TModel);
    }

    public async Task<IEnumerable<TModel>> GetAllAsync<TModel>() 
    {
        IEnumerable<TModel> foundUsers = await _usersRepository.GetAllAsync<TModel>();

        return foundUsers;
    }
}
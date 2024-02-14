using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.Csharp.Data.Database;
using Server.Csharp.Data.Models;

namespace Server.Csharp.Data.Repositories;

public class UsersRepository:GenericRepository<User>,IUsersRepository
{
    private readonly IMapper _mapper;
    public UsersRepository(ApplicationDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<User?> GetByEmailAndVerificationTokenAsync(string email, string token)
    {
        return await _context.Users.FirstOrDefaultAsync(
            u =>
                u.EmailVerificationToken != null &&
                u.Email == email &&
                u.EmailVerificationToken == token
        );
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(
            u => u.Email == email
        );
    }

    public async Task<User?> GetByRefreshTokenAsync(string token)
    {
        return await _context.Users.FirstOrDefaultAsync(
            u => u.Sessions.Any(rt => rt.RefreshToken == token)
        );
    }

    public async Task<bool> IsExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(
            u => u.Email == email
        );
    }

    public async Task<IEnumerable<TModel>> GetAllAsync<TModel>()
    {
        return await _context.Users.ProjectTo<TModel>(_mapper.ConfigurationProvider).ToListAsync();
    }
}
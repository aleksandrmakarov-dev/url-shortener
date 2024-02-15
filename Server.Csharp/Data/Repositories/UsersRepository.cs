using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.Csharp.Data.Database;
using Server.Csharp.Data.Entities;

namespace Server.Csharp.Data.Repositories;

public class UsersRepository:GenericRepository<User>,IUsersRepository
{
    public UsersRepository(ApplicationDbContext context, IMapper mapper) : base(context,mapper)
    {
    }

    public async Task<User?> GetByEmailAndVerificationTokenAsync(string email, string token)
    {
        return await Context.Users.FirstOrDefaultAsync(
            u =>
                u.EmailVerificationToken != null &&
                u.Email == email &&
                u.EmailVerificationToken == token
        );
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users.Include(u=>u.Role).FirstOrDefaultAsync(
            u => u.Email == email
        );
    }

    public override async Task<User?> GetByIdAsync(Guid id)
    {
        return await Context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> IsExistsByEmailAsync(string email)
    {
        return await Context.Users.AnyAsync(
            u => u.Email == email
        );
    }
}
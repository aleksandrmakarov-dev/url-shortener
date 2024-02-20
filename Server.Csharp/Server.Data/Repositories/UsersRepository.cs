using Microsoft.EntityFrameworkCore;
using Server.Data.Database;
using Server.Data.Entities;

namespace Server.Data.Repositories;

public class UsersRepository(ApplicationDbContext context) :GenericRepository<User>(context),IUsersRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

}
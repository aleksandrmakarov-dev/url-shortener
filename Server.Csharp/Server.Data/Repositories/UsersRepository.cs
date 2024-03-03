using Microsoft.EntityFrameworkCore;
using Server.Data.Database;
using Server.Data.Entities;

namespace Server.Data.Repositories;

public class UsersRepository(ApplicationDbContext context) :GenericRepository<User>(context),IUsersRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

}
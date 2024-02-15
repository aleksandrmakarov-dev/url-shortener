using Server.Csharp.Data.Entities;

namespace Server.Csharp.Data.Repositories
{
    public interface IUsersRepository: IGenericRepository<User>
    {
        Task<User?> GetByEmailAndVerificationTokenAsync(string email,string token);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> IsExistsByEmailAsync(string email);
    }
}

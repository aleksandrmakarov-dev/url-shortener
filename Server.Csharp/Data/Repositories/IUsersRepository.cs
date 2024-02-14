﻿using Server.Csharp.Data.Models;

namespace Server.Csharp.Data.Repositories
{
    public interface IUsersRepository: IGenericRepository<User>
    {
        Task<User?> GetByEmailAndVerificationTokenAsync(string email,string token);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRefreshTokenAsync(string token);
        Task<bool> IsExistsByEmailAsync(string email);
        Task<IEnumerable<TModel>> GetAllAsync<TModel>();
    }
}

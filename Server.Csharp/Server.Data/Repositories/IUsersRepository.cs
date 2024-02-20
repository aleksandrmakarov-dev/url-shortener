using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Data.Entities;

namespace Server.Data.Repositories
{
    public interface IUsersRepository: IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email); 
    }
}

using Server.Data.Database;
using Server.Data.Entities;

namespace Server.Data.Repositories;

public class NavigationsRepository : GenericRepository<Navigation>, INavigationsRepository
{
    public NavigationsRepository(ApplicationDbContext context):base(context)
    {
            
    }
}
using AutoMapper;
using Server.Csharp.Data.Database;
using Server.Csharp.Data.Entities;

namespace Server.Csharp.Data.Repositories;

public class ShortUrlsRepository:GenericRepository<ShortUrl>,IShortUrlsRepository
{
    public ShortUrlsRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
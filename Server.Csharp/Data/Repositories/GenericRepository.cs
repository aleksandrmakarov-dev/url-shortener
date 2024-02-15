using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.Csharp.Data.Database;
using Server.Csharp.Data.Entities;
using Server.Csharp.Data.Exceptions;

namespace Server.Csharp.Data.Repositories;

public class GenericRepository<TModel>:IGenericRepository<TModel> where TModel : ObjectId
{
    protected readonly ApplicationDbContext Context;
    protected readonly IMapper Mapper;

    public GenericRepository(ApplicationDbContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }


    public virtual async Task<TModel> CreateAsync(TModel model)
    {
        await Context.Set<TModel>().AddAsync(model);
        bool isCreated = await SaveChangesAsync();

        if (!isCreated) throw new DataException($"Exception occured while creating {nameof(TModel)} in database");

        return model;
    }

    public virtual async Task<TModel> UpdateAsync(TModel model)
    {
        Context.Set<TModel>().Update(model);
        bool isUpdated = await SaveChangesAsync();

        if (!isUpdated) throw new DataException($"Exception occured while updating {nameof(TModel)} in database");

        return model;
    }

    public virtual async Task DeleteAsync(TModel model)
    {
        Context.Set<TModel>().Remove(model);
        bool isDeleted = await SaveChangesAsync();

        if (!isDeleted) throw new DataException($"Exception occured while deleting {nameof(TModel)} in database");

    }

    public virtual async Task<TModel?> GetByIdAsync(Guid id)
    {
        return await Context.Set<TModel>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<TMapModel?> GetByIdAsync<TMapModel>(Guid id) where TMapModel : ObjectId
    {
        return await Context.Set<TModel>().ProjectTo<TMapModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<TModel>> GetAllAsync()
    {
        return await Context.Set<TModel>().ToListAsync();
    }

    public virtual async Task<IEnumerable<TMapModel>> GetAllAsync<TMapModel>()
    {
        return await Context.Set<TModel>().ProjectTo<TMapModel>(Mapper.ConfigurationProvider).ToListAsync();
    }

    private async Task<bool> SaveChangesAsync()
    {
        int rowsChanged = await Context.SaveChangesAsync();

        return rowsChanged > 0;
    }
}
using Microsoft.EntityFrameworkCore;
using Server.Csharp.Data.Database;
using Server.Csharp.Data.Exceptions;
using Server.Csharp.Data.Models;

namespace Server.Csharp.Data.Repositories;

public class GenericRepository<TModel>:IGenericRepository<TModel> where TModel : DomainObject
{
    protected readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<TModel> CreateAsync(TModel model)
    {
        await _context.Set<TModel>().AddAsync(model);
        bool isCreated = await SaveChangesAsync();

        if (!isCreated) throw new DataException($"Exception occured while creating {nameof(TModel)} in database");

        return model;
    }

    public async Task<TModel> UpdateAsync(TModel? model)
    {
        _context.Set<TModel>().Update(model);
        bool isUpdated = await SaveChangesAsync();

        if (!isUpdated) throw new DataException($"Exception occured while updating {nameof(TModel)} in database");

        return model;
    }

    public async Task DeleteAsync(TModel model)
    {
        _context.Set<TModel>().Remove(model);
        bool isDeleted = await SaveChangesAsync();

        if (!isDeleted) throw new DataException($"Exception occured while deleting {nameof(TModel)} in database");

    }

    public async Task<TModel?> GetByIdAsync(Guid id)
    {
        return await _context.Set<TModel>().FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        return await _context.Set<TModel>().ToListAsync();
    }

    private async Task<bool> SaveChangesAsync()
    {
        int rowsChanged = await _context.SaveChangesAsync();

        return rowsChanged > 0;
    }
}
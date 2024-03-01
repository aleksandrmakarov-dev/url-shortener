using System.Drawing;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Server.Data.Database;
using Server.Data.Entities;

namespace Server.Data.Repositories;

public class GenericRepository<TEntity>(ApplicationDbContext context) : IGenericRepository<TEntity>
    where TEntity : Entity
{
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);

        await SaveChangesAsync($"Failed to create {nameof(TEntity)}");

        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    { 
        context.Set<TEntity>().Update(entity);

        await SaveChangesAsync($"Failed to update {nameof(TEntity)}");

        return entity;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);

        await SaveChangesAsync($"Failed to delete {nameof(TEntity)}");
    }

    public virtual Task<TEntity?> GetByIdAsync(Guid id)
    {
        return context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await context.Set<TEntity>().OrderByDescending(e=>e.CreatedAt).ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetPageAsync(int page, int size,Expression<Func<TEntity,bool>>? whereExpression = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (whereExpression != null)
        {
            query = query.Where(whereExpression);
        }

        return await query
            .OrderByDescending(e=>e.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? whereExpression = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (whereExpression != null)
        {
            return await query.CountAsync(whereExpression);
        }

        return await query.CountAsync();
    }

    protected virtual async Task SaveChangesAsync(string? onErrorMessage)
    {
        int changes = await context.SaveChangesAsync();

        if (changes == 0)
        {
            throw new Exception(onErrorMessage);
        }
    }
}
using System.Linq.Expressions;
using Server.Data.Entities;

namespace Server.Data.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetPageAsync(int page, int size,
            Expression<Func<TEntity, bool>>? whereExpression = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? whereExpression = null);
    }
}

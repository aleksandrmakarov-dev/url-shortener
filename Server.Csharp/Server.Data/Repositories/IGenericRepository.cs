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
    }
}

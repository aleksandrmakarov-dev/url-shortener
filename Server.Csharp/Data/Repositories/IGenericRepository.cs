using Microsoft.AspNetCore.Components.Web;
using Server.Csharp.Data.Entities;

namespace Server.Csharp.Data.Repositories
{
    public interface IGenericRepository<TModel> where TModel : ObjectId
    {
        Task<TModel> CreateAsync(TModel model);
        Task<TModel> UpdateAsync(TModel model);
        Task DeleteAsync(TModel model);
        Task<TModel?> GetByIdAsync(Guid id);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<IEnumerable<TMapModel>> GetAllAsync<TMapModel>();
        Task<TMapModel?> GetByIdAsync<TMapModel>(Guid id) where TMapModel : ObjectId;
    }
}

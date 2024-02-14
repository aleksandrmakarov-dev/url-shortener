using Microsoft.AspNetCore.Components.Web;
using Server.Csharp.Data.Models;

namespace Server.Csharp.Data.Repositories
{
    public interface IGenericRepository<TModel> where TModel : DomainObject
    {
        Task<TModel> CreateAsync(TModel model);
        Task<TModel> UpdateAsync(TModel? model);
        Task DeleteAsync(TModel model);
        Task<TModel?> GetByIdAsync(Guid id);
        Task<IEnumerable<TModel>> GetAllAsync();
    }
}

using Server.Csharp.Business.Models.Responses;

namespace Server.Csharp.Business.Services
{
    public interface IUsersService
    {
        Task<TModel?> GetByIdAsync<TModel>(Guid id);
        Task<IEnumerable<TModel>> GetAllAsync<TModel>();
    }
}

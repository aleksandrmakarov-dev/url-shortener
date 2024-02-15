using Server.Csharp.Business.Models.Common;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Business.Models.Responses;

namespace Server.Csharp.Business.Services
{
    public interface IShortUrlsService
    {
        Task<ShortUrlResponse> CreateAsync(CreateShortUrlRequest request);
        Task<RedirectResponse> GetRedirectUrlAsync(Guid id);
        Task<IEnumerable<ShortUrlResponse>> GetAllAsync();
    }
}

using Server.Infrastructure.Models;
using Server.Infrastructure.Models.Filters;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Interfaces;

public interface IShortUrlsService
{
    Task<ShortUrlResponse> CreateAsync(CreateShortUrlRequest request);
    Task<ShortUrlResponse> GetByAliasAsync(GetShortUrlByAliasRequest request);
    Task<IEnumerable<ShortUrlResponse>> GetAllAsync();
    Task<ShortUrlResponse> UpdateByIdAsync(Guid id, UpdateShortUrlRequest request);
    Task<ShortUrlResponse> DeleteByIdAsync(Guid id);
    Task<ShortUrlResponse> GetByIdAsync(Guid id);
    Task<Paged<ShortUrlResponse>> GetPageAsync(int page, int size, ShortUrlsPageFilter? filter = null);

}
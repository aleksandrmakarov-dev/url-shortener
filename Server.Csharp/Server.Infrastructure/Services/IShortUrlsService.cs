using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services;

public interface IShortUrlsService
{
    Task<ShortUrlResponse> CreateAsync(CreateShortUrlRequest request);
    Task<ShortUrlResponse> GetByAliasAsync(string alias);
}
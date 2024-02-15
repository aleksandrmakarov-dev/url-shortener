using AutoMapper;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Business.Models.Responses;
using Server.Csharp.Data.Entities;
using Server.Csharp.Data.Repositories;
using Server.Csharp.Presentation.Exceptions;

namespace Server.Csharp.Business.Services;

public class EfShortUrlsService : IShortUrlsService
{
    private readonly IMapper _mapper;
    private readonly IShortUrlsRepository _shortUrlsRepository;

    public EfShortUrlsService(IShortUrlsRepository shortUrlsRepository, IMapper mapper)
    {
        _shortUrlsRepository = shortUrlsRepository;
        _mapper = mapper;
    }

    public async Task<ShortUrlResponse> CreateAsync(CreateShortUrlRequest request)
    {
        ShortUrl shortUrlToCreate = _mapper.Map<CreateShortUrlRequest, ShortUrl>(request);

        ShortUrl createdUrl = await _shortUrlsRepository.CreateAsync(shortUrlToCreate);

        return _mapper.Map<ShortUrl, ShortUrlResponse>(createdUrl);
    }

    public async Task<RedirectResponse> GetRedirectUrlAsync(Guid id)
    {
        ShortUrl? shortUrl = await _shortUrlsRepository.GetByIdAsync(id);

        if (shortUrl == null)
        {
            throw new NotFoundException($"Short URL '{id}' not found");
        }

        if (shortUrl.ExpiresAt != null && DateTime.UtcNow > shortUrl.ExpiresAt)
        {
            throw new BadRequestException("Url is expired");
        }

        RedirectResponse response = new RedirectResponse
        {
            Url = shortUrl.Original
        };

        return response;
    }

    public async Task<IEnumerable<ShortUrlResponse>> GetAllAsync()
    {
        return await _shortUrlsRepository.GetAllAsync<ShortUrlResponse>();
    }
}
using System.Linq.Expressions;
using AutoMapper;
using LinqKit;
using Server.Data.Common;
using Server.Data.Entities;
using Server.Data.Repositories;
using Server.Infrastructure.Exceptions;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models;
using Server.Infrastructure.Models.Filters;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services
{
    public class ShortUrlsService:IShortUrlsService
    {
        private readonly IShortUrlsRepository _shortUrlsRepository;
        private readonly IGenericCacheRepository<ShortUrl> _shortUrlsCacheRepository;
        private readonly ITokensService _tokensService;
        private readonly IMapper _mapper;

        public ShortUrlsService(
            IShortUrlsRepository shortUrlsRepository, 
            IMapper mapper, 
            ITokensService tokensService, 
            IGenericCacheRepository<ShortUrl> shortUrlsCacheRepository)
        {
            _shortUrlsRepository = shortUrlsRepository;
            _mapper = mapper;
            _tokensService = tokensService;
            _shortUrlsCacheRepository = shortUrlsCacheRepository;
        }

        public async Task<ShortUrlResponse> CreateAsync(CreateShortUrlRequest request)
        {
            ShortUrl shortUrl = _mapper.Map<CreateShortUrlRequest,ShortUrl>(request);

            // check if custom alias is set

            if (!string.IsNullOrEmpty(request.CustomAlias))
            {

                // check if custom alias is already exists
                bool isAliasExist = await _shortUrlsRepository.IsExistByAliasAsync(request.CustomAlias);

                if (isAliasExist)
                {
                    throw new BadRequestException($"Alias {request.CustomAlias} is already registered. Try another one");
                }

                shortUrl.Alias = request.CustomAlias;
            }
            else
            {
                // generate random alias

                string randomAlias = _tokensService.GetToken(8);

                //check if new alias is exist in database

                if (await _shortUrlsRepository.IsExistByAliasAsync(randomAlias))
                {
                    randomAlias = _tokensService.GetToken(8);
                }

                shortUrl.Alias = randomAlias;
            }


            ShortUrl createdShortUrl = await _shortUrlsRepository.CreateAsync(shortUrl);

            // cache short url value
            await _shortUrlsCacheRepository.SetAsync(createdShortUrl.Alias, createdShortUrl,Constants.CacheTimeToLive);

            ShortUrlResponse shortUrlResponse = _mapper.Map<ShortUrl, ShortUrlResponse>(createdShortUrl);

            return shortUrlResponse;
        }

        public async Task<ShortUrlResponse> GetByAliasAsync(GetShortUrlByAliasRequest request)
        {
            // check if url exists in cache
            ShortUrl? foundShortUrl = await _shortUrlsCacheRepository.GetAsync(request.Alias);

            // if cache value is null try get from database
            if (foundShortUrl == null)
            {
                foundShortUrl = await _shortUrlsRepository.GetByAliasAsync(request.Alias);
                // if value from database is not null, save it to cache
                if (foundShortUrl != null)
                {
                    await _shortUrlsCacheRepository.SetAsync(foundShortUrl.Alias, foundShortUrl,Constants.CacheTimeToLive);
                }
                // otherwise throw not found exception
                else
                {
                    throw new NotFoundException($"Short url with alias {request.Alias} not found");
                }
            }
            else
            {
                // if short url found in cache refresh ttl
                await _shortUrlsCacheRepository.RefreshAsync(request.Alias);
            }

            // if flag throwOnExpire is true and url is expired throw an exception

            if (request.ThrowOnExpire is true && foundShortUrl.ExpiresAt < DateTime.UtcNow)
            {
                throw new BadRequestException("Short URL is expired");
            }

            // add new navigation item

            ShortUrlResponse shortUrlResponse = _mapper.Map<ShortUrl,ShortUrlResponse>(foundShortUrl);

            shortUrlResponse.Domain = "http://localhost:5173";

            return shortUrlResponse;
        }

        public async Task<IEnumerable<ShortUrlResponse>> GetAllAsync()
        {
            IEnumerable<ShortUrl> foundShortUrls = await _shortUrlsRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ShortUrlResponse>>(foundShortUrls);
        }

        public async Task<ShortUrlResponse> UpdateByIdAsync(Guid id, UpdateShortUrlRequest request)
        {
            ShortUrl? foundShortUrl = await _shortUrlsRepository.GetByIdAsync(id);

            if (foundShortUrl == null)
            {
                throw new NotFoundException($"Short url with id {id} not found");
            }

            ShortUrl shortUrlToUpdate = _mapper.Map<UpdateShortUrlRequest, ShortUrl>(request,foundShortUrl);

            // check if custom alias is set

            if (!string.IsNullOrEmpty(request.CustomAlias))
            {
                if (request.CustomAlias != foundShortUrl.Alias)
                {
                    // check if custom alias is already exists
                    bool isAliasExist = await _shortUrlsRepository.IsExistByAliasAsync(request.CustomAlias);

                    if (isAliasExist)
                    {
                        throw new BadRequestException($"Alias {request.CustomAlias} is already registered. Try another one");
                    }

                    shortUrlToUpdate.Alias = request.CustomAlias;
                }
            }
            else
            {
                // generate random alias

                string randomAlias = _tokensService.GetToken(8);

                //check if new alias is exist in database

                if (await _shortUrlsRepository.IsExistByAliasAsync(randomAlias))
                {
                    randomAlias = _tokensService.GetToken(8);
                }

                shortUrlToUpdate.Alias = randomAlias;
            }
            

            ShortUrl updatedShortUrl = await _shortUrlsRepository.UpdateAsync(shortUrlToUpdate);

            // cache short url value
            await _shortUrlsCacheRepository.SetAsync(updatedShortUrl.Alias, updatedShortUrl,Constants.CacheTimeToLive);

            return _mapper.Map<ShortUrl, ShortUrlResponse>(updatedShortUrl);
        }

        public async Task<ShortUrlResponse> DeleteByIdAsync(Guid id)
        {
            ShortUrl? foundShortUrl = await _shortUrlsRepository.GetByIdAsync(id);

            if (foundShortUrl == null)
            {
                throw new NotFoundException($"Short url with id {id} not found");
            }

            await _shortUrlsRepository.DeleteAsync(foundShortUrl);

            // try to remove short url cache
            await _shortUrlsCacheRepository.DeleteAsync(foundShortUrl.Alias);

            return _mapper.Map<ShortUrl, ShortUrlResponse>(foundShortUrl);
        }

        public async Task<ShortUrlResponse> GetByIdAsync(Guid id)
        {
            ShortUrl? foundShortUrl = await _shortUrlsRepository.GetByIdAsync(id);

            if (foundShortUrl == null)
            {
                throw new NotFoundException($"Short Url with id {id} not found");
            }

            return _mapper.Map<ShortUrl, ShortUrlResponse>(foundShortUrl);
        }

        public async Task<Paged<ShortUrlResponse>> GetPageAsync(int page = 1, int size = 10, ShortUrlsPageFilter? filter = null)
        {
            // by default where expression is set to null
            Expression<Func<ShortUrl, bool>>? whereExpression = null;

            // if filter not equal to null check filter values
            if (filter != null)
            {
                
                if (filter.UserId != null)
                {
                    // if user id is not null create expression that checks if user id is equal to provided userId
                    Expression<Func<ShortUrl, bool>> filterByUserId = su => su.UserId == filter.UserId;
                    whereExpression = filterByUserId;
                }

                if (!string.IsNullOrEmpty(filter.Query))
                {
                    // if query is not null or empty string create expression that checks if alias contains query value
                    Expression<Func<ShortUrl, bool>> filterByAlias = su =>
                        su.Alias
                            .ToLower()
                            .Contains(filter.Query.ToLower()) || 
                        su.Original
                            .ToLower()
                            .Contains(filter.Query.ToLower());

                    // if whereExpression is null set it equal to filterByAlias expression otherwise use and with previous one
                    whereExpression = whereExpression != null ? whereExpression.And(filterByAlias) : filterByAlias;
                }
            }

            // get page result
            IEnumerable<ShortUrl> foundShortUrls = await _shortUrlsRepository.GetPageAsync(page, size,whereExpression);

            // count total number of items
            int count = await _shortUrlsRepository.CountAsync(whereExpression);

            IEnumerable<ShortUrlResponse> mappedShortUrls = _mapper.Map<IEnumerable<ShortUrlResponse>>(foundShortUrls);

            // return page result
            Paged<ShortUrlResponse> pagedResponse = new Paged<ShortUrlResponse>
            {
                Items = mappedShortUrls,
                Pagination = new Pagination
                {
                    Page = page,
                    Size = size,
                    // check if there are items left for the next page
                    HasNextPage = count > ((page - 1)*size)+size,
                    // check if page is bigger than 1
                    HasPreviousPage = page > 1
                }
            };

            return pagedResponse;
        }
    }
}

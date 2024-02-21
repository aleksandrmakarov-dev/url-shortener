using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Server.Data.Entities;
using Server.Data.Repositories;
using Server.Infrastructure.Exceptions;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services
{
    public class ShortUrlsService:IShortUrlsService
    {
        private readonly IShortUrlsRepository _shortUrlsRepository;
        private readonly ITokensService _tokensService;
        private readonly IMapper _mapper;

        public ShortUrlsService(IShortUrlsRepository shortUrlsRepository, IMapper mapper, ITokensService tokensService)
        {
            _shortUrlsRepository = shortUrlsRepository;
            _mapper = mapper;
            _tokensService = tokensService;
        }

        public async Task<ShortUrlResponse> CreateAsync(CreateShortUrlRequest request)
        {
            ShortUrl shortUrl = _mapper.Map<CreateShortUrlRequest,ShortUrl>(request);

            // check if custom alias is set

            if (request.CustomAlias != null)
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

            ShortUrlResponse shortUrlResponse = _mapper.Map<ShortUrl, ShortUrlResponse>(createdShortUrl);
            
            shortUrlResponse.Domain = "http://localhost:7153";

            return shortUrlResponse;
        }

        public async Task<ShortUrlResponse> GetByAliasAsync(string alias)
        {
            ShortUrl? foundShortUrl = await _shortUrlsRepository.GetByAliasAsync(alias);

            if (foundShortUrl == null)
            {
                throw new NotFoundException($"Short url with alias {alias} not found");
            }

            ShortUrlResponse shortUrlResponse = _mapper.Map<ShortUrl,ShortUrlResponse>(foundShortUrl);

            shortUrlResponse.Domain = "http://localhost:7153";

            return shortUrlResponse;
        }
    }
}

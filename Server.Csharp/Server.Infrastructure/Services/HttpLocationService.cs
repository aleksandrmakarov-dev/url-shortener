using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models.Responses;
using Server.Infrastructure.Options;

namespace Server.Infrastructure.Services
{
    public class HttpLocationService:ILocationService
    {
        private readonly LocationOptions _options;
        private readonly HttpClient _httpClient;

        public HttpLocationService(HttpClient httpClient, IOptions<LocationOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<LocationResponse?> GetByIpAddressAsync(string ipAddress)
        {
            string route = $"{_options.BaseUrl}/{ipAddress}";
            LocationResponse? location = await _httpClient.GetFromJsonAsync<LocationResponse>(route);

            return location;
        }
    }
}

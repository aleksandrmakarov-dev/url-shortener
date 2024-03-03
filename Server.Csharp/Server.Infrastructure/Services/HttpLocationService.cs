using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services
{
    public class HttpLocationService:ILocationService
    {
        private const string BaseUrl = "http://ip-api.com/json";
        private readonly HttpClient _httpClient;

        public HttpLocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LocationResponse?> GetByIpAddressAsync(string ipAddress)
        {
            string route = $"{BaseUrl}/{ipAddress}";
            LocationResponse? location = await _httpClient.GetFromJsonAsync<LocationResponse>(route);

            return location;
        }
    }
}

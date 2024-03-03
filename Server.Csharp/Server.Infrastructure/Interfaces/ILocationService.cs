using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Interfaces
{
    public interface ILocationService
    {
        Task<LocationResponse?> GetByIpAddressAsync(string ipAddress);
    }
}

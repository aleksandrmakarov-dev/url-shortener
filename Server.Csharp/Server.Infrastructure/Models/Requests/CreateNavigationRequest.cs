using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Requests
{
    public class CreateNavigationRequest
    {
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public string? IpAddress { get; set; }
        public string? Platform { get; set; }
        public string? Browser { get; set; }
        public Guid ShortUrlId { get; set; }
    }
}

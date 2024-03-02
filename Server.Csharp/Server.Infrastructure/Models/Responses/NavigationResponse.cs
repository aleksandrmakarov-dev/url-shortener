using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Responses
{
    public class NavigationResponse
    {
        public Guid Id { get; set; }
        public required string Country { get; set; }
        public string? IpAddress { get; set; }
        public DateTime NavigatedAt { get; set; }
    }
}

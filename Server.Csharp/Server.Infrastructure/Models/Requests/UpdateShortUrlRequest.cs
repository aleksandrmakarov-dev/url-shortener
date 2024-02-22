using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Requests
{
    public class UpdateShortUrlRequest
    {
        public string? Original { get; set; }
        public string? CustomAlias { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}

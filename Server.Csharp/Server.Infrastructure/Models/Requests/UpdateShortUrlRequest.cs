using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Requests
{
    public class UpdateShortUrlRequest
    {
        [Url]
        public string? Original { get; set; }
        [MaxLength(16)]
        [RegularExpression("^[A-Za-z0-9_-]*$")]
        public string? CustomAlias { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}

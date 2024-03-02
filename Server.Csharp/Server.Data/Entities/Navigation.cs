using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class Navigation:Entity
    {
        [MaxLength(16)]
        public required string IpAddress { get; set; }
        [MaxLength(5)]
        public required string CountryCode { get; set; }
        [MaxLength(64)]
        public required string CountryName { get; set; }
        [MaxLength(64)]
        public required string Platform { get; set; }
        [MaxLength(64)]
        public required string Browser { get; set; }
        public Guid ShortUrlId { get; set; }
        public ShortUrl? ShortUrl { get; set; }
    }
}

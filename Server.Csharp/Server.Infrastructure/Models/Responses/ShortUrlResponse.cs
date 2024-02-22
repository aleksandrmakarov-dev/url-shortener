using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Responses
{
    public class ShortUrlResponse
    {
        public Guid Id { get; set; }
        public required string Original { get; set; }
        public required string Alias { get; set; }
        public required string Domain { get; set; }
        public Guid? UserId { get; set; }
    }
}

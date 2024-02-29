using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Filters
{
    public class ShortUrlsPageFilter
    {
        public string? Query { get; set; }
        public Guid? UserId { get; set; }
    }
}

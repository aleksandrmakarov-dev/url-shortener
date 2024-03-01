using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Requests
{
    public class GetShortUrlByAliasRequest
    {
        public required string Alias { get; set; }
        public bool? ThrowOnExpire { get; set; }
    }
}

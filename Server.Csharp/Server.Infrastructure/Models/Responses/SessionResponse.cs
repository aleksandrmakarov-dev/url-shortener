using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Responses
{
    public class SessionResponse
    {
        public required string AccessToken { get; set; }
        public Guid UserId { get; set; }
    }
}

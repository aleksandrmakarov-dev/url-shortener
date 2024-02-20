using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models
{
    public class JwtPayload
    {
        public Guid Id { get; set; }
        public Role Role { get; set; }
    }
}

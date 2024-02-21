using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class Session:Entity
    {
        [MaxLength(256)]
        public required string RefreshToken{ get; set; }
        public DateTime ExpiresAt { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }

    }
}

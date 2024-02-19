using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public class ShortUrl:Entity
    {
        public required string Redirect { get; set; }
        public required string Alias { get; set;}
        public Guid UserId { get; set;}
        public User? User { get; set;}
        public DateTime? ExpiresAt { get; set; }
    }
}

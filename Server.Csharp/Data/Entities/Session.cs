using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Csharp.Data.Models
{
    public class Session:DomainObject
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }  
    }
}

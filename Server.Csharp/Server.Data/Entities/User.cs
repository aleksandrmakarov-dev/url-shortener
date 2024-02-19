using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities
{
    public class User : Entity
    {
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiresAt { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public ICollection<ShortUrl> ShortUrls { get; set; } = new List<ShortUrl>();   
    }
}

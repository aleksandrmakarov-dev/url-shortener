using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Models.Requests 
{
    public class CreateShortUrlRequest
    {
        [Required]
        [Url]
        public required string Original { get; set; }
        [MaxLength(16)]
        [RegularExpression("^[A-Za-z0-9_-]*$")]
        public string? CustomAlias { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}

namespace Server.Csharp.Data.Models
{
    public class User:DomainObject
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationExpiresAt { get; set; }
        public ICollection<Token> Tokens { get; set; } = new List<Token>();
        public ICollection<ShortUrl> ShortUrls { get; set; } = new List<ShortUrl>();
    }
}

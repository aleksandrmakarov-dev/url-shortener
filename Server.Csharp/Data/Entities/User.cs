namespace Server.Csharp.Data.Entities
{
    public class User:DomainObject
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public string? EmailVerificationToken { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime? EmailVerificationTokenExpiresAt { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<ShortUrl> ShortUrls { get; set; } = new List<ShortUrl>();
    }
}

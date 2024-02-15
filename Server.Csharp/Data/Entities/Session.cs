namespace Server.Csharp.Data.Entities
{
    public class Session:DomainObject
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public bool IsExpired => DateTime.UtcNow > RefreshTokenExpiresAt;
    }
}

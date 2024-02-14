namespace Server.Csharp.Data.Models
{
    public class ShortUrl:DomainObject
    {
        public string Original { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}

namespace Server.Csharp.Business.Models.Requests
{
    public class CreateShortUrlRequest
    {
        public string Original { get; set; }
        public string? Alias { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}

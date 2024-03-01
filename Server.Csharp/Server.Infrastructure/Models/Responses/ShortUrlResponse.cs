namespace Server.Infrastructure.Models.Responses
{
    public class ShortUrlResponse
    {
        public Guid Id { get; set; }
        public required string Original { get; set; }
        public required string Alias { get; set; }
        public required string Domain { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

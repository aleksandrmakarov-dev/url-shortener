namespace Server.Infrastructure.Models.Requests
{
    public class CreateNavigationRequest
    {
        public string? Country { get; set; }
        public string? IpAddress { get; set; }
        public string? Platform { get; set; }
        public string? Browser { get; set; }
        public Guid ShortUrlId { get; set; }
    }
}

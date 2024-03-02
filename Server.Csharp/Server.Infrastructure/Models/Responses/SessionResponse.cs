namespace Server.Infrastructure.Models.Responses
{
    public class SessionResponse
    {
        public required string AccessToken { get; set; }
        public Guid UserId { get; set; }
        public required string Email { get; set; }
        public Role Role { get; set; }
    }
}

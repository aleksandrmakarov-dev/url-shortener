namespace Server.Infrastructure.Models.Responses
{
    public class EmailVerificationResponse
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiresAt { get; set; }
    }
}

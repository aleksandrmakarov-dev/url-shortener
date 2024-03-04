namespace Server.Infrastructure.Options
{
    public class MailingOptions
    {
        public const string Name = "Mailing";
        public required string From { get; set; }
    }
}

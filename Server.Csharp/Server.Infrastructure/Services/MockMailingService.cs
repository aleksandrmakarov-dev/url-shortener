using Microsoft.Extensions.Options;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Options;

namespace Server.Infrastructure.Services
{
    public class MockMailingService:IMailingService
    {
        private readonly MailingOptions _options;

        public MockMailingService(IOptions<MailingOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            Console.WriteLine($"From: {_options.From}");
            Console.WriteLine($"To: {to}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {body}");

            await Task.FromResult(true);
        }
    }
}

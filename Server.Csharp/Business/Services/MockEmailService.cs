
using Server.Csharp.Business.Models.Common;

namespace Server.Csharp.Business.Services
{
    public class MockEmailService:IEmailService
    {
        public async Task SendAsync(SendEmailOptions options)
        {
            await Task.Delay(1000);

            Console.WriteLine($"To: {options.To}");
            Console.WriteLine($"From: {options.From}");
            Console.WriteLine($"Subject: {options.Subject}");
            Console.WriteLine($"Body: {options.Body}");
        }
    }
}

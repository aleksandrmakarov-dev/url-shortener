using Server.Csharp.Business.Models.Common;

namespace Server.Csharp.Business.Services
{
    public interface IEmailService
    {
        Task SendAsync(SendEmailOptions options);
    }
}

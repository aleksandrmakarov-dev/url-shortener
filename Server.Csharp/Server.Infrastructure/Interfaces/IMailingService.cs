using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Interfaces
{
    public interface IMailingService
    {
        Task SendAsync(string to, string subject, string body);
    }
}

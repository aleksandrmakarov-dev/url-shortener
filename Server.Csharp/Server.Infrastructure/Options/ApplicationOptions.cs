using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Options
{
    public class ApplicationOptions
    {
        public const string Name = "Application";
        public required string ClientBaseUrl { get; set; }
    }
}

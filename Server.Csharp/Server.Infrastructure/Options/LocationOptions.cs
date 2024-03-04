using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Options
{
    public class LocationOptions
    {
        public const string Name = "Location";
        public required string BaseUrl { get; set; }
    }
}

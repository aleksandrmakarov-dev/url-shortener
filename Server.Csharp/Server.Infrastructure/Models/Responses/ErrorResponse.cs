using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models.Responses
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public required string Error { get; set; }
        public required string Message { get; set; }
    }
}

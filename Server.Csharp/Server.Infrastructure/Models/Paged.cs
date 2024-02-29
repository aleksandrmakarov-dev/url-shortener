using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Models
{
    public class Paged<T>
    {
        public required IEnumerable<T> Items { get; set; }
        public required Pagination Pagination { get; set; }
    }
}

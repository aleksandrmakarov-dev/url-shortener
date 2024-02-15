using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Csharp.Data.Entities
{
    public abstract class DomainObject:ObjectId
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

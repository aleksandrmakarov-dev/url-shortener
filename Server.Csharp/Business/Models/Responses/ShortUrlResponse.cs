using Server.Csharp.Data.Entities;

namespace Server.Csharp.Business.Models.Responses
{
    public class ShortUrlResponse:ObjectId
    {
        public string Original { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}

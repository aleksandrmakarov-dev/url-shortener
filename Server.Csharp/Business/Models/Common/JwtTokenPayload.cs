
using Server.Csharp.Presentation.Common;

namespace Server.Csharp.Business.Models.Common
{
    public class JwtTokenPayload
    {
        public Guid  Id { get; set; }
        public Roles  Role { get; set; }
    }
}

using AutoMapper;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Data.Models;

namespace Server.Csharp.Business.Profiles
{
    public class AuthProfile:Profile
    {
        public AuthProfile()
        {
            CreateMap<SignUpRequest,User>();
        }
    }
}

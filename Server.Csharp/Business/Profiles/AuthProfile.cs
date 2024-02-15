using AutoMapper;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Data.Entities;

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

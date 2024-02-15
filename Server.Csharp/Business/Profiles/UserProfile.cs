using AutoMapper;
using Server.Csharp.Business.Models.Responses;
using Server.Csharp.Data.Entities;

namespace Server.Csharp.Business.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>();
        }
    }
}

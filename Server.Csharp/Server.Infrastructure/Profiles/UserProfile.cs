using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Server.Data.Entities;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<SignUpRequest, User>();

            CreateMap<NewEmailVerificationRequest, User>();

            CreateMap<User, UserResponse>();

            CreateMap<User, EmailVerificationResponse>();
        }
    }
}

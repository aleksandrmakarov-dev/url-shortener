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
    public class ShortUrlProfile:Profile
    {
        public ShortUrlProfile()
        {
            CreateMap<CreateShortUrlRequest, ShortUrl>();

            CreateMap<ShortUrl, ShortUrlResponse>()
                .ForMember(
                    d=>d.Domain,
                    cd=>cd.MapFrom(v=> "http://localhost:5173")
                    );
            CreateMap<UpdateShortUrlRequest, ShortUrl>();
        }
    }
}

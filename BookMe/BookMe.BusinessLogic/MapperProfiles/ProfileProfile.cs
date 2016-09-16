using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.MapperProfiles
{
    public class ProfileProfile : AutoMapper.Profile
    {
        public ProfileProfile()
        {
            this.CreateMap<Profile, ProfileDTO>();
            this.CreateMap<ProfileDTO, Profile>();
        }
    }
}
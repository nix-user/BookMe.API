using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookMe.BusinessLogic.DTO;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.MapperProfiles
{
    public class ProfileProfile : AutoMapper.Profile
    {
        public ProfileProfile()
        {
            this.CreateMap<ProfileModel, ProfileDTO>();
        }
    }
}
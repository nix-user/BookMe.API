using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.MapperProfiles
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            this.CreateMap<UserProfileModel, UserProfileDTO>();
            this.CreateMap<UserProfileDTO, UserProfileModel>();
        }
    }
}
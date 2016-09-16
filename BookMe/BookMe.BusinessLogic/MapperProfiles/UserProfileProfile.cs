using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.MapperProfiles
{
    public class UserProfileProfile : AutoMapper.Profile
    {
        public UserProfileProfile()
        {
            this.CreateMap<UserProfile, UserProfileDTO>();
            this.CreateMap<UserProfileDTO, UserProfile>();
        }
    }
}
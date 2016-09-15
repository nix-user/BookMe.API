using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.MapperProfiles
{
    public class ResourceProfile : AutoMapper.Profile
    {
        public ResourceProfile()
        {
            this.CreateMap<Resource, ResourceDTO>();
        }
    }
}
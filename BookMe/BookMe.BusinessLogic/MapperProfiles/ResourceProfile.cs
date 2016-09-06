using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.MapperProfiles
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            this.CreateMap<Resource, ResourceDTO>(); 
        }
    }
}

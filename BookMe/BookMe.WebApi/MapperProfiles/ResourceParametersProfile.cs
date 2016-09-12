using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.MapperProfiles
{
    public class ResourceParametersProfile : Profile
    {
        public ResourceParametersProfile()
        {
            this.CreateMap<RoomFilterParameters, ResourceFilterParameters>()
                .ForMember(nameof(ResourceFilterParameters.RoomSize),
                    opt => opt.MapFrom(parameters => parameters.IsLarge ? RoomSizeDTO.Large : (RoomSizeDTO?)null));
        }
    }
}
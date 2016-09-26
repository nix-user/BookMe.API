using System;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Enums;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.MapperProfiles
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            this.CreateMap<Resource, ResourceDTO>()
                .ForMember(nameof(ResourceDTO.RoomSize), opt => opt.MapFrom(s => (RoomSizeDTO)(int)(s.RoomSize ?? RoomSize.Middle)));
            this.CreateMap<ResourceDTO, Resource>()
                .ForMember(nameof(Resource.RoomSize), opt => opt.MapFrom(s => (RoomSize)(int)(s.RoomSize ?? RoomSizeDTO.Middle)));
        }
    }
}
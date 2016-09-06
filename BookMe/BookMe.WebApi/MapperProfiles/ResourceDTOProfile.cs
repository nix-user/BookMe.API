﻿using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.MapperProfiles
{
    public class ResourceDTOProfile : Profile
    {
        public ResourceDTOProfile()
        {
            CreateMap<ResourceDTO, Room>()
            .ForMember(nameof(Room.IsBig), opt => opt.MapFrom(resource => resource.RoomSize == RoomSize.Large))
            .ForMember(nameof(Room.IsHasPolykom), opt => opt.MapFrom(resource => resource.HasPolycom))
            .ForMember(nameof(Room.Number), opt => opt.MapFrom(resource => resource.Title));
        }
    }
}
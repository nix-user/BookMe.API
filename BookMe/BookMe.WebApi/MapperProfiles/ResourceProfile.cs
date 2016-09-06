using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.MapperProfiles
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<Resource, Room>()
            .ForMember(nameof(Room.IsBig), opt => opt.MapFrom(resource => resource.RoomSize == RoomSize.Large))
            .ForMember(nameof(Room.IsHasPolykom), opt => opt.MapFrom(resource => resource.HasPolycom))
            .ForMember(nameof(Room.Number), opt => opt.MapFrom(resource => resource.Title));
        }
    }
}
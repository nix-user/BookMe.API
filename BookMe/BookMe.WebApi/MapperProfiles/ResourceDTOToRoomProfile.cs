using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.MapperProfiles
{
    public class ResourceDTOToRoomProfile : Profile
    {
        public ResourceDTOToRoomProfile()
        {
            this.CreateMap<ResourceDTO, Room>()
            .ForMember(nameof(Room.IsBig), opt => opt.MapFrom(resource => resource.RoomSize == RoomSizeDTO.Large))
            .ForMember(nameof(Room.IsHasPolykom), opt => opt.MapFrom(resource => resource.HasPolycom))
            .ForMember(nameof(Room.Number), opt => opt.MapFrom(resource => resource.Title));
        }
    }
}
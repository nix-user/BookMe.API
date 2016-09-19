using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Enums;

namespace BookMe.BusinessLogic.MapperProfiles
{
    public class RoomSizeProfile : Profile
    {
        public RoomSizeProfile()
        {
            this.CreateMap<RoomSizeDTO?, RoomSize?>();
        }
    }
}
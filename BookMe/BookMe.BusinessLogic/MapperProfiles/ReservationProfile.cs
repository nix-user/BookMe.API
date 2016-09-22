using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.MapperProfiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            this.CreateMap<Reservation, ReservationDTO>()
                .ForMember(dest => dest.TextPeriod, opt => opt.MapFrom(x => x.ToString()))
                .ForMember(dest => dest.TextRule, opt => opt
                .MapFrom(x => x.RecurrenceData != null ? x.RecurrenceData.ToString() : string.Empty));
            this.CreateMap<ReservationDTO, Reservation>();
        }
    }
}
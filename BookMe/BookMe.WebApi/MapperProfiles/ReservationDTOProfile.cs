using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.MapperProfiles
{
    public class ReservationDTOProfile : Profile
    {
        public ReservationDTOProfile()
        {
            this.CreateMap<ReservationModel, ReservationDTO>()
                .ForMember(nameof(ReservationDTO.OwnerName), opt => opt.MapFrom(reservation => reservation.Author))
                .ForMember(nameof(ReservationDTO.EventDate), opt => opt.MapFrom(reservation => reservation.From))
                .ForMember(nameof(ReservationDTO.EndDate), opt => opt.MapFrom(reservation => reservation.To))
                .ForMember(nameof(ReservationDTO.IsRecurrence), opt => opt.MapFrom(reservation => reservation.IsRecursive));

            this.CreateMap<ReservationDTO, ReservationModel>()
                .ForMember(nameof(ReservationModel.Author), opt => opt.MapFrom(reservation => reservation.OwnerName))
                .ForMember(nameof(ReservationModel.From), opt => opt.MapFrom(reservation => reservation.EventDate))
                .ForMember(nameof(ReservationModel.To), opt => opt.MapFrom(reservation => reservation.EndDate))
                .ForMember(nameof(ReservationModel.IsRecursive), opt => opt.MapFrom(reservation => reservation.IsRecurrence))
                .ForMember(nameof(ReservationModel.Room), opt => opt.MapFrom(reservation => reservation.Resource));
        }
    }
}

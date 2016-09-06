using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.MapperProfiles
{
    public class ReservationToReservationDTOProfile : Profile
    {
        public ReservationToReservationDTOProfile()
        {
            this.CreateMap<ReservationModel, ReservationDTO>();
        }
    }
}
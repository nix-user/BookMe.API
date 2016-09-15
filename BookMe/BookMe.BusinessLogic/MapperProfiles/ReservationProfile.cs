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
    public class ReservationProfile : AutoMapper.Profile
    {
        public ReservationProfile()
        {
            this.CreateMap<Reservation, ReservationDTO>();
            this.CreateMap<ReservationDTO, Reservation>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookMe.BusinessLogic.DTO;

namespace BookMe.WebApi.Models
{
    public class UserReservationsModel
    {
        public IEnumerable<ReservationModel> TodayReservations { get; set; }

        public IEnumerable<ReservationModel> AllReservations { get; set; }
    }
}
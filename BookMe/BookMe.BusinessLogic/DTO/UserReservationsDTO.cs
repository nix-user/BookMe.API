using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.BusinessLogic.DTO
{
    public class UserReservationsDTO
    {
        public IEnumerable<ReservationDTO> TodayReservations { get; set; }

        public IEnumerable<ReservationDTO> AllReservations { get; set; }
    }
}
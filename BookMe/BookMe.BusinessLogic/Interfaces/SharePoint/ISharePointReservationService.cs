using System;
using System.Collections.Generic;
using BookMe.BusinessLogic.DTO;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointReservationService
    {
        IEnumerable<ReservationDTO> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd);

        IEnumerable<ReservationDTO> GetUserActiveReservations(string userName);
    }
}

using System;
using System.Collections.Generic;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointReservationService
    {
        IEnumerable<Reservation> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd);

        IEnumerable<Reservation> GetUserActiveReservations(string userName);
    }
}

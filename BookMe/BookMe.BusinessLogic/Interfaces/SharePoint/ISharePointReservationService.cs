using System;
using System.Collections.Generic;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointReservationService
    {
        IEnumerable<Reservation> GetPossibleIntersectingReservations(DateTime intervalStart, DateTime intervalEnd);
    }
}

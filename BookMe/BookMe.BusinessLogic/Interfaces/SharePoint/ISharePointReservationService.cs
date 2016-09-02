using System;
using BookMe.Core.Models;
using System.Collections;
using System.Collections.Generic;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointReservationService
    {
        IEnumerable<Reservation> GetPossibleIntersectingReservations(DateTime intervalStart, DateTime intervalEnd);
    }
}

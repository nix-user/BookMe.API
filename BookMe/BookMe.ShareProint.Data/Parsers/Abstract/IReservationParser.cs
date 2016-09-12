using System;
using System.Collections.Generic;
using BookMe.Core.Models;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public interface IReservationParser
    {
        ListItemCollection GetAllPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd);

        ListItemCollection GetPossibleRoomReservationsInInterval(DateTime intervalStart, DateTime intervalEnd, int roomId);

        ListItemCollection GetUserActiveReservations(string userName);

        void AddReservation(Reservation reservation);
    }
}

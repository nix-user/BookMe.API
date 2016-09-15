using System;
using System.Collections.Generic;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public interface IReservationParser
    {
        IEnumerable<ListItem> GetAllPossibleReservationsInInterval(Interval interval);

        IEnumerable<ListItem> GetPossibleRoomReservationsInInterval(Interval interval, int roomId);

        IEnumerable<ListItem> GetUserActiveReservations(string userName);

        void AddReservation(IDictionary<string, object> reservation);
    }
}
using System;
using System.Collections.Generic;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public interface IReservationParser
    {
        IEnumerable<ListItem> GetPossibleReservationsInInterval(Interval interval, IEnumerable<string> resourceNames);

        IEnumerable<ListItem> GetUserActiveReservations(string userName);

        IEnumerable<ListItem> GetUserReservations(string userName, Interval interval);

        void AddReservation(IDictionary<string, object> reservation);

        void RemoveReservation(int reservationId);
    }
}
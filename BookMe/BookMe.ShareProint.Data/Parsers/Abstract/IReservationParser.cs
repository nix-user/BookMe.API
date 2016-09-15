﻿using System;
using System.Collections.Generic;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public interface IReservationParser
    {
        IEnumerable<ListItem> GetPossibleReservationsInInterval(Interval interval, int? roomId);

        IEnumerable<ListItem> GetUserActiveReservations(string userName);

        void AddReservation(Reservation reservation);
    }
}
using System;
using System.Collections.Generic;
using BookMe.Core.Models;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public interface IReservationParser
    {
        ListItemCollection GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd);

        ListItemCollection GetUserActiveReservations(string userName);
    }
}

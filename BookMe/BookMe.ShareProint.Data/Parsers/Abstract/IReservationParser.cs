using System;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public interface IReservationParser
    {
        ListItemCollection GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd);
    }
}

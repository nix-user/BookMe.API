using System;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Converters.Abstract
{
    public interface IReservationParser
    {
        ListItemCollection GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd);
    }
}

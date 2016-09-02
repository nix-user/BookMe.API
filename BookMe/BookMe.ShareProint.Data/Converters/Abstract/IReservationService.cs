using System;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Converters.Abstract
{
    public interface IReservationParser
    {
        ListItemCollection GetPossibleIntersecting(DateTime intervalStart, DateTime intervalEnd);
    }
}

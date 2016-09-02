using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookMe.ShareProint.Data.Converters.Abstract;
using CamlexNET;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers
{
    public class ReservationParser : BaseParser, IReservationParser
    {
        private const string ReservationStartFieldName = "EventDate";
        private const string ReservationEndFieldName = "EndDate";
        private const string RecurrentFieldName = "fRecurrence";

        public ListItemCollection GetPossibleIntersecting(DateTime intervalStart, DateTime intervalEnd)
        {
            var reservationsList = this.context.Web.Lists.GetByTitle(ListNames.Reservations);

            Expression<Func<ListItem, bool>> recurrentBookingCondition =
                reservation => (bool)reservation[RecurrentFieldName] && (DateTime)reservation[ReservationEndFieldName] > DateTime.Now;

            Expression<Func<ListItem, bool>> regularBookingCondition = reservation => !(bool)reservation[RecurrentFieldName]
            && (DateTime)reservation[ReservationStartFieldName] < intervalEnd && (DateTime)reservation[ReservationEndFieldName] > intervalStart;

            var queryConditions = Camlex.Query().WhereAny(new List<Expression<Func<ListItem, bool>>>() { recurrentBookingCondition, regularBookingCondition });
            ListItemCollection items = reservationsList.GetItems(this.CreateCamlQuery(queryConditions.ToString()));

            return this.LoadCollectionFromServer(items);
        }
    }
}

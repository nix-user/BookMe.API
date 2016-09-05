using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BookMe.ShareProint.Data.Parsers.Abstract;
using CamlexNET;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Concrete
{
    public class ReservationParser : BaseParser, IReservationParser
    {
        private const string ReservationStartFieldName = "EventDate";
        private const string ReservationEndFieldName = "EndDate";
        private const string RecurrentFieldName = "fRecurrence";

        public ReservationParser(ClientContext context) : base(context)
        {
        }

        public ListItemCollection GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd)
        {
            var reservationsList = this.Context.Web.Lists.GetByTitle(ListNames.Reservations);

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

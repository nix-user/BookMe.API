using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Parsers.Abstract;
using CamlexNET;
using Microsoft.SharePoint.Client;
using Mono.Linq.Expressions;

namespace BookMe.ShareProint.Data.Parsers.Concrete
{
    public class ReservationParser : BaseParser, IReservationParser
    {
        private const string ReservationStartFieldName = "EventDate";
        private const string ReservationEndFieldName = "EndDate";
        private const string RecurrentFieldName = "fRecurrence";
        private const string AuthorFieldName = "Author";
        private const string Facilities = "Facilities";

        public ReservationParser(ClientContext context) : base(context)
        {
        }

        public ListItemCollection GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd, int? roomId = null)
        {
            try
            {
                var reservationsList = this.Context.Web.Lists.GetByTitle(ListNames.Reservations);

                Expression<Func<ListItem, bool>> reservationsRetrievalCondition =
                        this.GetRecurrentBookingCondition(roomId)
                        .OrElse(this.GetRegularReservationCondition(intervalStart, intervalEnd, roomId));

                var queryConditions = Camlex.Query().Where(reservationsRetrievalCondition);
                ListItemCollection items = reservationsList.GetItems(this.CreateCamlQuery(queryConditions.ToString()));
                return this.LoadCollectionFromServer(items);
            }
            catch
            {
                throw new ParserException(RetrivalErrorMessage);
            }
        }

        public ListItemCollection GetUserActiveReservations(string userName)
        {
            try
            {
                var reservationsList = this.Context.Web.Lists.GetByTitle(ListNames.Reservations);
                var queryConditions =
                    Camlex.Query()
                        .Where(reservation => (string)reservation[AuthorFieldName] == userName && (DateTime)reservation[ReservationEndFieldName] > DateTime.Now);
                ListItemCollection items = reservationsList.GetItems(this.CreateCamlQuery(queryConditions.ToString()));
                return this.LoadCollectionFromServer(items);
            }
            catch
            {
                throw new ParserException(RetrivalErrorMessage);
            }
        }

        public void AddReservation(Reservation reservation)
        {
        }

        private Expression<Func<ListItem, bool>> GetRecurrentBookingCondition(int? roomId)
        {
            Expression<Func<ListItem, bool>> recurrentCondition = reservation => (bool)reservation[RecurrentFieldName] && (DateTime)reservation[ReservationEndFieldName] > DateTime.Now;
            if (roomId != null)
            {
                recurrentCondition = recurrentCondition.AndAlso(reservation => reservation[Facilities] == (DataTypes.LookupId)roomId.Value.ToString());
            }

            return recurrentCondition;
        }

        private Expression<Func<ListItem, bool>> GetRegularReservationCondition(DateTime intervalStart, DateTime intervalEnd, int? roomId)
        {
            Expression<Func<ListItem, bool>> regularReservationCondition = reservation => !(bool)reservation[RecurrentFieldName]
               && (DateTime)reservation[ReservationStartFieldName] < intervalEnd &&
               (DateTime)reservation[ReservationEndFieldName] > intervalStart;
            if (roomId != null)
            {
                regularReservationCondition = regularReservationCondition.AndAlso(reservation => reservation[Facilities] == (DataTypes.LookupId)roomId.Value.ToString());
            }

            return regularReservationCondition;
        }
    }
}

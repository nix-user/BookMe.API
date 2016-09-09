﻿using System;
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

        public ListItemCollection GetAllPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd)
        {
            return this.GetPossibleReservationsInInterval(intervalStart, intervalEnd);
        }

        public ListItemCollection GetPossibleRoomReservationsInInterval(DateTime intervalStart, DateTime intervalEnd, int roomId)
        {
            return this.GetPossibleReservationsInInterval(intervalStart, intervalEnd, roomId);
        }

        public ListItemCollection GetUserActiveReservations(string userName)
        {
            return this.GetReservations(reservation => (string)reservation[AuthorFieldName] == userName && (DateTime)reservation[ReservationEndFieldName] > DateTime.Now);
        }

        public void AddReservation(Reservation reservation)
        {
        }

        private ListItemCollection GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd, int? roomId = null)
        {
            Expression<Func<ListItem, bool>> reservationsRetrievalCondition = this.GetRecurrentReservationCondition(intervalEnd, roomId)
                    .OrElse(this.GetRegularReservationCondition(intervalStart, intervalEnd, roomId));
            return this.GetReservations(reservationsRetrievalCondition);
        }

        private ListItemCollection GetReservations(Expression<Func<ListItem, bool>> conditions)
        {
            try
            {
                var reservationsList = this.Context.Web.Lists.GetByTitle(ListNames.Reservations);
                ListItemCollection items = reservationsList.GetItems(this.CreateCamlQuery(Camlex.Query().Where(conditions).ToString()));
                return this.LoadCollectionFromServer(items);
            }
            catch
            {
                throw new ParserException(RetrivalErrorMessage);
            }
        }

        private Expression<Func<ListItem, bool>> GetRecurrentReservationCondition(DateTime intervalEnd, int? roomId)
        {
            Expression<Func<ListItem, bool>> recurrentCondition = reservation => (bool)reservation[RecurrentFieldName]
            && (DateTime)reservation[ReservationStartFieldName] < intervalEnd
            && (DateTime)reservation[ReservationEndFieldName] > DateTime.Now;
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

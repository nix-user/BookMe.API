using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
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

        protected List ReservartionList
        {
            get
            {
                return this.Context.Web.Lists.GetByTitle(ListNames.Reservations);
            }
        }

        public ReservationParser(ClientContext context) : base(context)
        {
        }

        public ListItemCollection GetAllPossibleReservationsInInterval(Interval interval)
        {
            return this.GetPossibleReservationsInInterval(interval);
        }

        public ListItemCollection GetPossibleRoomReservationsInInterval(Interval interval, int roomId)
        {
            return this.GetPossibleReservationsInInterval(interval, roomId);
        }

        public ListItemCollection GetUserActiveReservations(string userName)
        {
            return this.GetReservations(reservation => (string)reservation[AuthorFieldName] == userName && (DateTime)reservation[ReservationEndFieldName] > DateTime.Now);
        }

        public void AddReservation(IDictionary<string, object> reservatioFieldValues)
        {
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem newItem = this.ReservartionList.AddItem(itemCreateInfo);

            foreach (var key in reservatioFieldValues.Keys)
            {
                newItem[key] = reservatioFieldValues[key];
            }

            newItem.Update();
            this.Context.ExecuteQuery();
        }

        private ListItemCollection GetPossibleReservationsInInterval(Interval interval, int? roomId = null)
        {
            Expression<Func<ListItem, bool>> reservationsRetrievalCondition = this.GetRecurrentReservationCondition(interval, roomId)
                    .OrElse(this.GetRegularReservationCondition(interval, roomId));
            return this.GetReservations(reservationsRetrievalCondition);
        }

        private ListItemCollection GetReservations(Expression<Func<ListItem, bool>> conditions)
        {
            try
            {
                ListItemCollection items = this.ReservartionList.GetItems(this.CreateCamlQuery(Camlex.Query().Where(conditions).ToString()));
                return this.LoadCollectionFromServer(items);
            }
            catch (Exception e)
            {
                throw new ParserException(RetrivalErrorMessage, e);
            }
        }

        private Expression<Func<ListItem, bool>> GetRecurrentReservationCondition(Interval interval, int? roomId)
        {
            Expression<Func<ListItem, bool>> recurrentCondition = reservation => (bool)reservation[RecurrentFieldName]
            && (DateTime)reservation[ReservationStartFieldName] < interval.End
            && (DateTime)reservation[ReservationEndFieldName] > DateTime.Now;
            if (roomId != null)
            {
                recurrentCondition = recurrentCondition.AndAlso(reservation => reservation[Facilities] == (DataTypes.LookupId)roomId.Value.ToString());
            }

            return recurrentCondition;
        }

        private Expression<Func<ListItem, bool>> GetRegularReservationCondition(Interval interval, int? roomId)
        {
            Expression<Func<ListItem, bool>> regularReservationCondition = reservation => !(bool)reservation[RecurrentFieldName]
            && (DateTime)reservation[ReservationStartFieldName] < interval.End
            && (DateTime)reservation[ReservationEndFieldName] > DateTime.Now;

            if (roomId != null)
            {
                regularReservationCondition = regularReservationCondition.AndAlso(reservation => reservation[Facilities] == (DataTypes.LookupId)roomId.Value.ToString());
            }

            return regularReservationCondition;
        }
    }
}
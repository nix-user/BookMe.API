using System;
using System.Collections.Generic;
using System.Linq;
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
        private const string Duration = "Duration";

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

        public IEnumerable<ListItem> GetAllPossibleReservationsInInterval(Interval interval)
        {
            return this.GetPossibleReservationsInInterval(interval);
        }

        public IEnumerable<ListItem> GetPossibleRoomReservationsInInterval(Interval interval, int roomId)
        {
            return this.GetPossibleReservationsInInterval(interval, roomId);
        }

        public IEnumerable<ListItem> GetUserActiveReservations(string userName)
        {
            return this.GetReservations(reservation => (string)reservation[AuthorFieldName] == userName && (DateTime)reservation[ReservationEndFieldName] > DateTime.Today.AddDays(-1));
        }

        public void AddReservation(IDictionary<string, object> reservatioFieldValues)
        {
            try
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
            catch (Exception e)
            {
                throw new ParserException(AddingErrorMessage, e);
            }
        }

        private IEnumerable<ListItem> GetPossibleReservationsInInterval(Interval interval, int? roomId = null)
        {
            Expression<Func<ListItem, bool>> reservationsRetrievalCondition = this.GetRecurrentReservationCondition(roomId).OrElse(this.GetRegularReservationCondition(interval, roomId));
            return this.GetReservations(reservationsRetrievalCondition);
        }

        private IEnumerable<ListItem> GetReservations(Expression<Func<ListItem, bool>> conditions)
        {
            try
            {
                ListItemCollection items = this.ReservartionList.GetItems(this.CreateCamlQuery(Camlex.Query().Where(conditions).ToString()));
                return this.LoadCollectionFromServer(items).ToList().Where(reservation => (reservation[Facilities] as IList<FieldLookupValue>)?[0].LookupId != null && reservation[Duration] != null);
            }
            catch (Exception e)
            {
                throw new ParserException(RetrivalErrorMessage, e);
            }
        }

        private Expression<Func<ListItem, bool>> GetRecurrentReservationCondition(int? roomId)
        {
            Expression<Func<ListItem, bool>> recurrentCondition = reservation => (bool)reservation[RecurrentFieldName];
            if (roomId != null)
            {
                recurrentCondition = recurrentCondition.AndAlso(reservation => reservation[Facilities] == (DataTypes.LookupId)roomId.Value.ToString());
            }

            return recurrentCondition;
        }

        private Expression<Func<ListItem, bool>> GetRegularReservationCondition(Interval interval, int? roomId)
        {
            Expression<Func<ListItem, bool>> regularReservationCondition = reservation => !(bool)reservation[RecurrentFieldName]
            && (DateTime)reservation[ReservationStartFieldName] <= interval.End
            && (DateTime)reservation[ReservationEndFieldName] > DateTime.Today.AddDays(-1);

            if (roomId != null)
            {
                regularReservationCondition = regularReservationCondition.AndAlso(reservation => reservation[Facilities] == (DataTypes.LookupId)roomId.Value.ToString());
            }

            return regularReservationCondition;
        }
    }
}
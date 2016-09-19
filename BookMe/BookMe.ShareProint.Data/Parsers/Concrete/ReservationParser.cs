﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookMe.Auth.Providers.Abstract;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Constants;
using BookMe.ShareProint.Data.Parsers.Abstract;
using CamlexNET;
using CamlexNET.Impl.Helpers;
using Microsoft.SharePoint.Client;
using Mono.Linq.Expressions;

namespace BookMe.ShareProint.Data.Parsers.Concrete
{
    public class ReservationParser : BaseParser, IReservationParser
    {
        protected List ReservartionList
        {
            get
            {
                return this.Context.Web.Lists.GetByTitle(ListNames.Reservations);
            }
        }

        public ReservationParser(ClientContext context, ICredentialsProvider credentialsProvider) : base(context, credentialsProvider)
        {
        }

        public IEnumerable<ListItem> GetPossibleReservationsInIntervalOptimized(Interval interval, IEnumerable<int> resourcesIds)
        {
            /*Expression<Func<ListItem, bool>> reservationsRetrievalCondition = this.GetRecurrentReservationCondition(null)
                .OrElse(this.GetRegularReservationCondition(interval, null)).AndAlso(this.GetRoomsFilteringCondition(interval, resourcesIds.ToList()));*/
            return this.GetReservations(this.GetRoomsFilteringCondition(interval, resourcesIds.ToList()));
        }

        public IEnumerable<ListItem> GetUserActiveReservations(string userName)
        {
            return this.GetReservations(reservation => (string)reservation[FieldNames.AuthorKey] == userName && (DateTime)reservation[FieldNames.EndDateKey] > DateTime.Today.AddDays(-1));
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

        public void RemoveReservation(int reservationId)
        {
            Expression<Func<ListItem, bool>> neededReservationCondition = reservation => (int)reservation[FieldNames.IdKey] == reservationId;
            var neededReservation = this.GetReservations(neededReservationCondition).FirstOrDefault();
            neededReservation.DeleteObject();
            try
            {
                this.Context.ExecuteQuery();
            }
            catch (Exception e)
            {
                throw new ParserException(e.Message, e);
            }
        }

        public IEnumerable<ListItem> GetPossibleReservationsInInterval(Interval interval, int? roomId)
        {
            Expression<Func<ListItem, bool>> reservationsRetrievalCondition = this.GetRecurrentReservationCondition(roomId).OrElse(this.GetRegularReservationCondition(interval, roomId));
            return this.GetReservations(reservationsRetrievalCondition);
        }

        private IEnumerable<ListItem> GetReservations(Expression<Func<ListItem, bool>> conditions)
        {
            try
            {
                ListItemCollection items = this.ReservartionList.GetItems(this.GetCamlquery(conditions));
                var a = this.GetCamlquery(conditions);
                return this.LoadCollectionFromServer(items).ToList().Where(reservation => (reservation[FieldNames.FacilitiesKey] as IList<FieldLookupValue>)?[0].LookupId != null);
            }
            catch (Exception e)
            {
                throw new ParserException(RetrivalErrorMessage, e);
            }
        }

        private CamlQuery GetCamlquery(Expression<Func<ListItem, bool>> conditions)
        {
            var query = Camlex.Query().ViewFields(x => new object[]
                {
                    x[FieldNames.IdKey],
                    x[FieldNames.TitleKey],
                    x[FieldNames.DescriptionKey],
                    x[FieldNames.FacilitiesKey],
                    x[FieldNames.EventDateKey],
                    x[FieldNames.EndDateKey],
                    x[FieldNames.DurationKey],
                    x[FieldNames.IsRecurrenceKey],
                    x[FieldNames.AuthorKey],
                    x[FieldNames.ParrentIdKey],
                    x[FieldNames.EventTypeKey],
                    x[FieldNames.IsAllDayEventKey],
                    x[FieldNames.RecurrenceDate],
                    x[FieldNames.RecurrenceDataKey]
                }).Where(conditions).ToString();
            return this.CreateCamlQuery(query);
        }

        private Expression<Func<ListItem, bool>> GetRecurrentReservationCondition(int? roomId)
        {
            Expression<Func<ListItem, bool>> recurrentCondition = reservation => (bool)reservation[FieldNames.IsRecurrenceKey];
            if (roomId != null)
            {
                recurrentCondition = recurrentCondition.AndAlso(reservation => reservation[FieldNames.FacilitiesKey] == (DataTypes.LookupId)roomId.Value.ToString());
            }

            return recurrentCondition;
        }

        private Expression<Func<ListItem, bool>> GetRegularReservationCondition(Interval interval, int? roomId)
        {
            Expression<Func<ListItem, bool>> regularReservationCondition =
                reservation => !(bool)reservation[FieldNames.IsRecurrenceKey]
                               && (DateTime)reservation[FieldNames.EventDateKey] <= interval.End
                               && (DateTime)reservation[FieldNames.EndDateKey] >= interval.Start;

            if (roomId != null)
            {
                regularReservationCondition = regularReservationCondition.AndAlso(reservation => reservation[FieldNames.FacilitiesKey] == (DataTypes.LookupId)roomId.Value.ToString());
            }

            return regularReservationCondition;
        }

        private Expression<Func<ListItem, bool>> GetRoomsFilteringCondition(Interval interval, IList<int> roomsIds)
        {
            var languageConditions = new List<Expression<Func<ListItem, bool>>>();
            languageConditions.Add(x => (string)x["Language"] == "Russian");
            languageConditions.Add(x => (string)x["Language"] == "English");
            var langExpr = ExpressionsHelper.CombineOr(languageConditions);

            // (Language = Russian or Language = English) and
            // (FileLeafRef contains “.docx” or FileLeafRef contains “.xlsx” or ...)
            var expressions = new List<Expression<Func<ListItem, bool>>>();
            expressions.Add(langExpr);

            string query = CamlexNET.Camlex.Query().WhereAll(expressions).ToString();

            if (roomsIds.Any())
            {
                List<Expression<Func<ListItem, bool>>> roomFilteringExpressions = new List<Expression<Func<ListItem, bool>>>();
                for (int i = 1; i < roomsIds.Count(); i++)
                {
                    int roomId = roomsIds[i];
                    roomFilteringExpressions.Add(reservation => reservation[FieldNames.FacilitiesKey] == (DataTypes.LookupId)roomId.ToString());
                }

                var combinedExpressions = ExpressionsHelper.CombineOr(roomFilteringExpressions);
                var listWithCombExpression = new List<Expression<Func<ListItem, bool>>>();
                listWithCombExpression.Add(combinedExpressions);
                var myQuery = CamlexNET.Camlex.Query().WhereAll(listWithCombExpression).ToString();
                return ExpressionsHelper.CombineOr(roomFilteringExpressions);
            }

            return null;
        }
    }
}
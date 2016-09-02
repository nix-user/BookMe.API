using System;
using System.Collections.Generic;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services
{
    public class ReservationService : ISharePointReservationService
    {
        private IConverter<ListItem, Reservation> reservationConverter;
        private IReservationParser reservationParser;

        public ReservationService(IConverter<ListItem, Reservation> reservationConverter, IReservationParser reservationParser)
        {
            this.reservationConverter = reservationConverter;
            this.reservationParser = reservationParser;
        }

        public IEnumerable<Reservation> GetPossibleIntersectingReservations(DateTime intervalStart, DateTime intervalEnd)
        {
            var reservationsList = this.reservationParser.GetPossibleIntersecting(intervalStart, intervalEnd);
            return this.reservationConverter.Convert(reservationsList);
        }
    }
}

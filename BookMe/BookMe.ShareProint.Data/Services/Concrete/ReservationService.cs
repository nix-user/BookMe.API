using System;
using System.Collections.Generic;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services.Concrete
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

        public IEnumerable<Reservation> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd)
        {
            var reservationsList = this.reservationParser.GetPossibleReservationsInInterval(intervalStart, intervalEnd);
            return this.reservationConverter.Convert(reservationsList);
        }

        public IEnumerable<Reservation> GetUserActiveResevations(string userName)
        {
            var userActiveReservations = this.reservationParser.GetUserActiveResevations(userName);
            return this.reservationConverter.Convert(userActiveReservations);
        }
    }
}

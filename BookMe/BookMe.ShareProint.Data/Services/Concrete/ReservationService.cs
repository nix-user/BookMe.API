using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services.Concrete
{
    public class ReservationService : ISharePointReservationService
    {
        private IConverter<IDictionary<string, object>, Reservation> reservationConverter;
        private IReservationParser reservationParser;

        public ReservationService(IConverter<IDictionary<string, object>, Reservation> reservationConverter, IReservationParser reservationParser)
        {
            this.reservationConverter = reservationConverter;
            this.reservationParser = reservationParser;
        }

        public IEnumerable<ReservationDTO> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd)
        {
            var reservationsList = this.reservationParser
                .GetPossibleReservationsInInterval(intervalStart, intervalEnd).ToList()
                .Select(x => x.FieldValues);
            return this.reservationConverter.Convert(reservationsList).Select(Mapper.Map<Reservation, ReservationDTO>);
        }

        public IEnumerable<ReservationDTO> GetUserActiveReservations(string userName)
        {
            var userActiveReservations = this.reservationParser
                .GetUserActiveReservations(userName).ToList()
                .Select(x => x.FieldValues);
            return this.reservationConverter.Convert(userActiveReservations).Select(Mapper.Map<Reservation, ReservationDTO>);
        }
    }
}

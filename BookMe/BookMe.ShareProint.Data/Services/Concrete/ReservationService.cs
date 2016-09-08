using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers;
using BookMe.ShareProint.Data.Parsers.Abstract;
using BookMe.ShareProint.Data.Services.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services.Concrete
{
    public class ReservationService : BaseService, ISharePointReservationService
    {
        public ReservationService(IConverter<IDictionary<string, object>, Resource> resourceConverter,
            IConverter<IDictionary<string, object>, Reservation> reservationConverter,
            IResourceParser resourceParser,
            IReservationParser reservationParser) : base(resourceConverter, reservationConverter, resourceParser, reservationParser)
        {
        }

        public OperationResult<IEnumerable<ReservationDTO>> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd)
        {
            bool isReservationRetrievalSuccessful;
            var reservations = this.GetPossibleReservationsInInterval(intervalStart, intervalEnd, out isReservationRetrievalSuccessful);
            return new OperationResult<IEnumerable<ReservationDTO>>()
            {
                IsSuccessful = isReservationRetrievalSuccessful,
                Result = reservations?.Select(Mapper.Map<Reservation, ReservationDTO>)
            };
        }

        public OperationResult<IEnumerable<ReservationDTO>> GetUserActiveReservations(string userName)
        {
            bool isReservationRetrievalSuccessful;
            var reservations = this.GetUserActiveReservations(userName, out isReservationRetrievalSuccessful).ToList();
            bool isReservationsMappingSuccessful;
            var mappedReservations = this.DeeplyMapReservationsToReservationDTOs(reservations, out isReservationsMappingSuccessful);
            return new OperationResult<IEnumerable<ReservationDTO>>()
            {
                IsSuccessful = isReservationRetrievalSuccessful,
                Result = mappedReservations
            };
        }

        public OperationResult AddReservation(ReservationDTO reservationDTO)
        {
            return new OperationResult() { IsSuccessful = true };
        }
    }
}

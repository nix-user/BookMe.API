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
        private IConverter<IDictionary<string, object>, Reservation> reservationConverter;
        private IReservationParser reservationParser;

        public ReservationService(IConverter<IDictionary<string, object>, Resource> resourceConverter,
            IConverter<IDictionary<string, object>, Reservation> reservationConverter,
            IResourceParser resourceParser,
            IReservationParser reservationParser) : base(resourceConverter, reservationConverter, resourceParser, reservationParser)
        {
            this.reservationConverter = reservationConverter;
            this.reservationParser = reservationParser;
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
            try
            {
                var userActiveReservations = this.reservationParser
                    .GetUserActiveReservations(userName).ToList()
                    .Select(x => x.FieldValues);
                var reservations = this.reservationConverter.Convert(userActiveReservations).ToList();
                var convertedReservations = reservations.Select(Mapper.Map<Reservation, ReservationDTO>).ToList();
                this.FillRoomInReservationDTO(reservations, convertedReservations);
                return new OperationResult<IEnumerable<ReservationDTO>>()
                {
                    IsSuccessful = true,
                    Result = convertedReservations
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<ReservationDTO>>() { IsSuccessful = false };
            }
        }

        public OperationResult AddReservation(ReservationDTO reservationDTO)
        {
            return new OperationResult() { IsSuccessful = true };
        }

        private void FillRoomInReservationDTO(IEnumerable<Reservation> sharePointReservations, IEnumerable<ReservationDTO> convertedReservations)
        {
            /*var allResources = this.resourceService.GetAll().Result;
            for (int i = 0; i < convertedReservations.Count(); i++)
            {
                convertedReservations.ElementAt(i).Resource = allResources.FirstOrDefault(resource => resource.Id == sharePointReservations.ElementAt(i).ResourceId);
            }*/
        }
    }
}

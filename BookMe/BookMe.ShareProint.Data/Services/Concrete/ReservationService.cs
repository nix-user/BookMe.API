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
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services.Concrete
{
    public class ReservationService : ISharePointReservationService
    {
        private IConverter<IDictionary<string, object>, Reservation> reservationConverter;
        private IReservationParser reservationParser;
        private ISharePointResourceService resourceService;

        public ReservationService(IConverter<IDictionary<string, object>, Reservation> reservationConverter, IReservationParser reservationParser, ISharePointResourceService resourceService)
        {
            this.reservationConverter = reservationConverter;
            this.reservationParser = reservationParser;
            this.resourceService = resourceService;
        }

        public OperationResult<IEnumerable<ReservationDTO>> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd)
        {
            try
            {
                var reservationsList = this.reservationParser
                    .GetPossibleReservationsInInterval(intervalStart, intervalEnd).ToList()
                    .Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<ReservationDTO>>()
                {
                    IsSuccessful = true,
                    Result = this.reservationConverter.Convert(reservationsList).Select(Mapper.Map<Reservation, ReservationDTO>)
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<ReservationDTO>>() { IsSuccessful = false };
            }
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
            var allResources = this.resourceService.GetAll().Result;
            for (int i = 0; i < convertedReservations.Count(); i++)
            {
                convertedReservations.ElementAt(i).Resource = allResources.FirstOrDefault(resource => resource.Id == sharePointReservations.ElementAt(i).ResourceId);
            }
        }
    }
}

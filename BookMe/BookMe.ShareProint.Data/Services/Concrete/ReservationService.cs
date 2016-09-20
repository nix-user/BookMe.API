using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
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

        public OperationResult<IEnumerable<ReservationDTO>> GetPossibleReservationsInInterval(IntervalDTO interval, IEnumerable<ResourceDTO> resources)
        {
            var mappedResources = resources.Select(Mapper.Map<ResourceDTO, Resource>);
            var reservationsRetrieval = this.GetPossibleReservationsInIntervalFromParser(Mapper.Map<IntervalDTO, Interval>(interval), mappedResources);
            if (!reservationsRetrieval.IsSuccessful)
            {
                return new OperationResult<IEnumerable<ReservationDTO>>() { IsSuccessful = false };
            }

            var reservationsMapping = this.DeeplyMapReservationsToReservationDTOs(reservationsRetrieval.Result.ToList());
            return new OperationResult<IEnumerable<ReservationDTO>>()
            {
                IsSuccessful = reservationsRetrieval.IsSuccessful && reservationsMapping.IsSuccessful,
                Result = reservationsMapping.Result
            };
        }

        public OperationResult<IEnumerable<ReservationDTO>> GetUserActiveReservations(string userName)
        {
            var reservationsRetrieval = this.GetUserActiveReservationsFromParser(userName);
            var reservationsMapping = this.DeeplyMapReservationsToReservationDTOs(reservationsRetrieval.Result.ToList());
            return new OperationResult<IEnumerable<ReservationDTO>>()
            {
                IsSuccessful = reservationsRetrieval.IsSuccessful && reservationsMapping.IsSuccessful,
                Result = reservationsMapping.Result
            };
        }

        public OperationResult AddReservation(ReservationDTO reservation)
        {
            try
            {
                var convertedReservation = this.ReservationConverter.ConvertBack(Mapper.Map<ReservationDTO, Reservation>(reservation));
                this.ReservationParser.AddReservation(convertedReservation);
            }
            catch (ParserException)
            {
                return new OperationResult() { IsSuccessful = false };
            }

            return new OperationResult() { IsSuccessful = true };
        }

        public OperationResult RemoveReservation(int reservationId)
        {
            try
            {
                this.ReservationParser.RemoveReservation(reservationId);
            }
            catch (ParserException)
            {
                return new OperationResult() { IsSuccessful = false };
            }

            return new OperationResult() { IsSuccessful = true };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers;
using BookMe.ShareProint.Data.Parsers.Abstract;

namespace BookMe.ShareProint.Data.Services.Abstract
{
    public abstract class BaseService
    {
        private readonly IResourceParser resourceParser;
        private readonly IReservationParser reservationParser;
        private IConverter<IDictionary<string, object>, Resource> resourcesConverter;
        private IConverter<IDictionary<string, object>, Reservation> reservationConverter;

        protected BaseService(
            IConverter<IDictionary<string, object>, Resource> resourcesConverter,
            IConverter<IDictionary<string, object>, Reservation> reservationConverter,
            IResourceParser resourceParser,
            IReservationParser reservationParser)
        {
            this.resourceParser = resourceParser;
            this.reservationParser = reservationParser;
            this.resourcesConverter = resourcesConverter;
            this.reservationConverter = reservationConverter;
        }

        protected OperationResult<IEnumerable<Resource>> GetAllResources()
        {
            try
            {
                var resourceDictionariesCollection = this.resourceParser.GetAll().ToList().Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<Resource>>
                {
                    IsSuccessful = true,
                    Result = this.resourcesConverter.Convert(resourceDictionariesCollection)
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<Resource>> { IsSuccessful = false };
            }
        }

        protected OperationResult<IEnumerable<Reservation>> GetPossibleReservationsInIntervalFromParser(DateTime intervalStart, DateTime intervalEnd)
        {
            try
            {
                var reservationsList = this.reservationParser
                    .GetAllPossibleReservationsInInterval(intervalStart, intervalEnd).ToList()
                    .Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<Reservation>>
                {
                    IsSuccessful = true,
                    Result = this.reservationConverter.Convert(reservationsList)
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<Reservation>> { IsSuccessful = false };
            }
        }

        protected OperationResult<IEnumerable<Reservation>> GetPossibleRoomReservationsInIntervalFromParser(DateTime intervalStart, DateTime intervalEnd, int roomId)
        {
            try
            {
                var reservationsList = this.reservationParser
                    .GetPossibleRoomReservationsInInterval(intervalStart, intervalEnd, roomId).ToList()
                    .Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<Reservation>>
                {
                    IsSuccessful = true,
                    Result = this.reservationConverter.Convert(reservationsList)
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<Reservation>> { IsSuccessful = false };
            }
        }

        protected OperationResult<IEnumerable<Reservation>> GetUserActiveReservationsFromParser(string userName)
        {
            try
            {
                var userActiveReservations = this.reservationParser.GetUserActiveReservations(userName).ToList().Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<Reservation>>
                {
                    IsSuccessful = true,
                    Result = this.reservationConverter.Convert(userActiveReservations)
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<Reservation>> { IsSuccessful = false };
            }
        }

        // cannot user automapper Map because i need to get operation status
        protected OperationResult<IEnumerable<ReservationDTO>> DeeplyMapReservationsToReservationDTOs(IList<Reservation> sharePointReservations)
        {
            if (sharePointReservations == null)
            {
                return new OperationResult<IEnumerable<ReservationDTO>> { IsSuccessful = false };
            }

            var allResourcesRetrievalResult = this.GetAllResources();
            if (!allResourcesRetrievalResult.IsSuccessful)
            {
                return new OperationResult<IEnumerable<ReservationDTO>> { IsSuccessful = false };
            }

            var convertedReservations = sharePointReservations.Select(Mapper.Map<Reservation, ReservationDTO>).ToList();
            for (int i = 0; i < convertedReservations.Count(); i++)
            {
                var reservationResource = allResourcesRetrievalResult.Result.FirstOrDefault(resource => resource.Id == sharePointReservations[i].ResourceId);
                if (reservationResource != null)
                {
                    convertedReservations[i].Resource = Mapper.Map<Resource, ResourceDTO>(reservationResource);
                }
            }

            return new OperationResult<IEnumerable<ReservationDTO>>
            {
                IsSuccessful = true,
                Result = convertedReservations
            };
        }
    }
}

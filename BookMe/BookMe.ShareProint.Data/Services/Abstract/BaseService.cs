using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
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

        protected OperationResult<IEnumerable<Reservation>> GetPossibleReservationsInIntervalFromParser(Interval interval, int? roomId = null)
        {
            try
            {
                var reservationsDictionary = this.reservationParser
                    .GetPossibleReservationsInInterval(interval, roomId).ToList()
                    .Select(x => x.FieldValues);

                var reservationsList = this.reservationConverter.Convert(reservationsDictionary).ToList();

                List<Reservation> intersectingReservations = new List<Reservation>();
                foreach (var reservation in reservationsList)
                {
                    if (!reservation.IsRecurrence
                        || reservation.EventType == EventType.Modified
                        || (reservation.EventType == EventType.Recurrent && !this.WasRecurrentReservationModifiedOrDeletedOnGivenDay(reservation, reservationsList, interval.Start)))
                    {
                        var reservationBusyInterval = reservation.GetBusyInterval(interval.Start);
                        if (reservationBusyInterval != null && reservationBusyInterval.IsIntersecting(interval))
                        {
                            intersectingReservations.Add(reservation);
                        }
                    }
                }

                return new OperationResult<IEnumerable<Reservation>>
                {
                    IsSuccessful = true,
                    Result = intersectingReservations
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

        private bool WasRecurrentReservationModifiedOrDeletedOnGivenDay(Reservation reservationToCheck, IEnumerable<Reservation> allReservations, DateTime day)
        {
            if (reservationToCheck.EventType != EventType.Recurrent)
            {
                return false;
            }

            return allReservations.Any(reservation => reservation.ParentId == reservationToCheck.Id && reservation.RecurrenceDate.Value.Date == day.Date);
        }
    }
}
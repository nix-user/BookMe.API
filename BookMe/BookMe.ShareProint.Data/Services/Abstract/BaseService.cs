using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers;
using BookMe.ShareProint.Data.Parsers.Abstract;

namespace BookMe.ShareProint.Data.Services.Abstract
{
    public abstract class BaseService
    {
        protected IResourceParser ResourceParser { get; set; }

        protected IReservationParser ReservationParser { get; set; }

        protected IConverter<IDictionary<string, object>, Resource> ResourcesConverter { get; set; }

        protected IConverter<IDictionary<string, object>, Reservation> ReservationConverter { get; set; }

        protected BaseService(
            IConverter<IDictionary<string, object>, Resource> resourcesConverter,
            IConverter<IDictionary<string, object>, Reservation> reservationConverter,
            IResourceParser resourceParser,
            IReservationParser reservationParser)
        {
            this.ResourceParser = resourceParser;
            this.ReservationParser = reservationParser;
            this.ResourcesConverter = resourcesConverter;
            this.ReservationConverter = reservationConverter;
        }

        protected OperationResult<IEnumerable<Resource>> GetAllResources()
        {
            try
            {
                var resourceDictionariesCollection = this.ResourceParser.GetAll().ToList().Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<Resource>>
                {
                    IsSuccessful = true,
                    Result = this.ResourcesConverter.Convert(resourceDictionariesCollection)
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<Resource>> { IsSuccessful = false };
            }
        }

        protected OperationResult<IEnumerable<Reservation>> GetPossibleReservationsInIntervalFromParser(Interval interval)
        {
            try
            {
                var reservationsDictionary = this.ReservationParser
                    .GetAllPossibleReservationsInInterval(interval).ToList()
                    .Select(x => x.FieldValues);

                var reservationsList = this.ReservationConverter.Convert(reservationsDictionary);
                List<Reservation> reservationsInInterval = new List<Reservation>();
                foreach (var reservation in reservationsList)
                {
                    for (DateTime date = interval.Start; date <= interval.End; date = date.AddDays(1))
                    {
                        var busyInterval = reservation.GetBusyInterval(date);
                        if (busyInterval != null && busyInterval.IsIntersecting(interval))
                        {
                            reservationsInInterval.Add(reservation);
                        }
                    }
                }

                return new OperationResult<IEnumerable<Reservation>>
                {
                    IsSuccessful = true,
                    Result = reservationsInInterval
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<Reservation>> { IsSuccessful = false };
            }
        }

        protected OperationResult<IEnumerable<Reservation>> GetPossibleRoomReservationsInIntervalFromParser(Interval interval, int roomId)
        {
            try
            {
                var reservationsList = this.ReservationParser
                    .GetPossibleRoomReservationsInInterval(interval, roomId).ToList()
                    .Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<Reservation>>
                {
                    IsSuccessful = true,
                    Result = this.ReservationConverter.Convert(reservationsList)
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
                var userActiveReservations = this.ReservationParser.GetUserActiveReservations(userName).ToList().Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<Reservation>>
                {
                    IsSuccessful = true,
                    Result = this.ReservationConverter.Convert(userActiveReservations)
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
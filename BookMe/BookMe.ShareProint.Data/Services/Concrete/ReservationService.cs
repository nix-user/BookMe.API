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
<<<<<<< HEAD
        private IConverter<IDictionary<string, object>, Reservation> reservationConverter;
        private IReservationParser reservationParser;

        public ReservationService(IConverter<IDictionary<string, object>, Reservation> reservationConverter, IReservationParser reservationParser)
        {
            this.reservationConverter = reservationConverter;
            this.reservationParser = reservationParser;
=======
        public ReservationService(IConverter<IDictionary<string, object>, Resource> resourceConverter,
            IConverter<IDictionary<string, object>, Reservation> reservationConverter,
            IResourceParser resourceParser,
            IReservationParser reservationParser) : base(resourceConverter, reservationConverter, resourceParser, reservationParser)
        {
>>>>>>> master
        }

        public OperationResult<IEnumerable<ReservationDTO>> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd)
        {
            var reservationsRetrieval = this.GetPossibleReservationsInIntervalFromParser(intervalStart, intervalEnd);
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
<<<<<<< HEAD
                var userActiveReservations = this.reservationParser
                    .GetUserActiveReservations(userName).ToList()
                    .Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<ReservationDTO>>()
                {
                    IsSuccessful = true,
                    Result = this.reservationConverter.Convert(userActiveReservations).Select(Mapper.Map<Reservation, ReservationDTO>)
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<ReservationDTO>>() { IsSuccessful = false };
            }
=======
                IsSuccessful = reservationsRetrieval.IsSuccessful && reservationsMapping.IsSuccessful,
                Result = reservationsMapping.Result
            };
>>>>>>> master
        }

        public OperationResult AddReservation(ReservationDTO reservationDTO)
        {
            return new OperationResult() { IsSuccessful = true };
        }
    }
}

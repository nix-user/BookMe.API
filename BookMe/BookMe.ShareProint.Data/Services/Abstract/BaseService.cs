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

        protected IEnumerable<Resource> GetAllResources(out bool isSuccessful)
        {
            try
            {
                var resourceDictionariesCollection = this.resourceParser.GetAll().ToList().Select(x => x.FieldValues);
                isSuccessful = true;
                return this.resourcesConverter.Convert(resourceDictionariesCollection);
            }
            catch (ParserException)
            {
                isSuccessful = false;
                return null;
            }
        }

        protected IEnumerable<Reservation> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd, out bool isSuccessful)
        {
            try
            {
                var reservationsList = this.reservationParser
                    .GetPossibleReservationsInInterval(intervalStart, intervalEnd).ToList()
                    .Select(x => x.FieldValues);
                isSuccessful = true;
                return this.reservationConverter.Convert(reservationsList);
            }
            catch (ParserException)
            {
                isSuccessful = false;
                return null;
            }
        }

        protected IEnumerable<ReservationDTO> GetUserActiveReservations(string userName, out bool isSuccessful)
        {
            try
            {
                var userActiveReservations = this.reservationParser.GetUserActiveReservations(userName).ToList().Select(x => x.FieldValues);
                var reservations = this.reservationConverter.Convert(userActiveReservations).ToList();
                var convertedReservations = reservations.Select(Mapper.Map<Reservation, ReservationDTO>).ToList();

                bool isFillReservationsSuccessful;
                this.FillRoomInReservationDTO(reservations, convertedReservations, out isFillReservationsSuccessful);
                if (isFillReservationsSuccessful)
                {
                    isSuccessful = true;
                    return convertedReservations;
                }

                isSuccessful = false;
                return null;
            }
            catch (ParserException)
            {
                isSuccessful = false;
                return null;
            }
        }

        private void FillRoomInReservationDTO(IList<Reservation> sharePointReservations, IList<ReservationDTO> convertedReservations, out bool isSucessful)
        {
            bool isResourceRetrievalSuccessful;
            var allResources = this.GetAllResources(out isResourceRetrievalSuccessful).ToList();
            if (!isResourceRetrievalSuccessful)
            {
                isSucessful = false;
                return;
            }

            isSucessful = true;
            for (int i = 0; i < convertedReservations.Count(); i++)
            {
                var reservationResource = allResources.FirstOrDefault(resource => resource.Id == sharePointReservations[i].ResourceId);
                if (reservationResource != null)
                {
                    convertedReservations[i].Resource = Mapper.Map<Resource, ResourceDTO>(reservationResource);
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers;
using BookMe.ShareProint.Data.Parsers.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services.Concrete
{
    public class ResourceService : ISharePointResourceService
    {
        private IConverter<IDictionary<string, object>, Resource> resourceConverter;
        private IConverter<IDictionary<string, object>, Reservation> reservationConverter;
        private IResourceParser resourceParser;
        private IReservationParser reservationParser;

        public ResourceService(IConverter<IDictionary<string, object>, Resource> resourceConverter,
            IConverter<IDictionary<string, object>, Reservation> reservationConverter,
            IResourceParser resourceParser,
            IReservationParser reservationParser)
        {
            this.resourceConverter = resourceConverter;
            this.reservationConverter = reservationConverter;
            this.resourceParser = resourceParser;
            this.reservationParser = reservationParser;
        }

        public OperationResult<IEnumerable<ResourceDTO>> GetAll()
        {
            try
            {
                var resourceDictionariesCollection = this.resourceParser.GetAll().ToList().Select(x => x.FieldValues);
                return new OperationResult<IEnumerable<ResourceDTO>>()
                {
                    IsSuccessful = true,
                    Result =
                        this.resourceConverter.Convert(resourceDictionariesCollection)
                            .Select(Mapper.Map<Resource, ResourceDTO>)
                };
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<ResourceDTO>>()
                {
                    IsSuccessful = false
                };
            }
        }

        public OperationResult<IEnumerable<ResourceDTO>> GetAvailbleResources(ResourceFilterParameters resourceFilterParameters)
        {
            IEnumerable<Dictionary<string, object>> reservationsList;
            IEnumerable<Dictionary<string, object>> allResources;
            try
            {
                reservationsList = this.reservationParser
                    .GetPossibleReservationsInInterval(resourceFilterParameters.From, resourceFilterParameters.To).ToList()
                    .Select(x => x.FieldValues);

                allResources = this.resourceParser.GetAll().ToList().Select(x => x.FieldValues);
            }
            catch (ParserException)
            {
                return new OperationResult<IEnumerable<ResourceDTO>>() { IsSuccessful = false };
            }

            var convertedReservationsList = this.reservationConverter.Convert(reservationsList).ToList();

            var convertedResourcesList = this.resourceConverter.Convert(allResources).ToList();

            var availableResources = convertedResourcesList.Where(resource => this.DoesResourceMatchFilterParameters(resource, resourceFilterParameters) &&
                                !this.IsResourceAlreadyInUse(convertedReservationsList, resource));

            return new OperationResult<IEnumerable<ResourceDTO>>()
            {
                IsSuccessful = true,
                Result = availableResources.Select(Mapper.Map<Resource, ResourceDTO>)
            };
        }

        private bool DoesResourceMatchFilterParameters(Resource resource, ResourceFilterParameters filterParameters)
        {
            if (filterParameters.HasPolycom && !resource.HasPolycom)
            {
                return false;
            }

            if (filterParameters.RoomSize.HasValue &&
                resource.RoomSize.HasValue &&
                Mapper.Map<RoomSizeDTO, RoomSize>(filterParameters.RoomSize.Value) != resource.RoomSize.Value)
            {
                return false;
            }

            return true;
        }

        private bool IsResourceAlreadyInUse(List<Reservation> reservationsInSameInterval, Resource resource)
        {
            return reservationsInSameInterval.Any(reservation => reservation.ResourceId == resource.Id);
        }
    }
}
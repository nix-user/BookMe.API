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
using BookMe.ShareProint.Data.Services.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services.Concrete
{
    public class ResourceService : BaseService, ISharePointResourceService
    {
        private IConverter<IDictionary<string, object>, Resource> resourceConverter;
        private IConverter<IDictionary<string, object>, Reservation> reservationConverter;
        private IResourceParser resourceParser;
        private IReservationParser reservationParser;

        public ResourceService(IConverter<IDictionary<string, object>, Resource> resourceConverter,
            IConverter<IDictionary<string, object>, Reservation> reservationConverter,
            IResourceParser resourceParser,
            IReservationParser reservationParser) : base(resourceConverter, reservationConverter, resourceParser, reservationParser)
        {
            this.resourceConverter = resourceConverter;
            this.reservationConverter = reservationConverter;
            this.resourceParser = resourceParser;
            this.reservationParser = reservationParser;
        }

        public OperationResult<IEnumerable<ResourceDTO>> GetAll()
        {
            bool areResourcesSuccessfullyRetrieved;
            var resources = this.GetAllResources(out areResourcesSuccessfullyRetrieved);
            return new OperationResult<IEnumerable<ResourceDTO>>()
            {
                IsSuccessful = areResourcesSuccessfullyRetrieved,
                Result = resources?.Select(Mapper.Map<Resource, ResourceDTO>)
            };
        }

        public OperationResult<IEnumerable<ResourceDTO>> GetAvailbleResources(ResourceFilterParameters resourceFilterParameters)
        {
            bool isAllResourceRetrievalSuccessful;
            var allResources = this.GetAllResources(out isAllResourceRetrievalSuccessful);
            if (!isAllResourceRetrievalSuccessful)
            {
                return new OperationResult<IEnumerable<ResourceDTO>>() { IsSuccessful = false };
            }

            bool isPossibleReservationRetrievalSuccessful;
            var possibleReservationInInterval = this.GetPossibleReservationsInInterval(resourceFilterParameters.From, resourceFilterParameters.To, out isPossibleReservationRetrievalSuccessful);
            if (!isPossibleReservationRetrievalSuccessful)
            {
                return new OperationResult<IEnumerable<ResourceDTO>>() { IsSuccessful = false };
            }

            var availableResources = allResources.Where(resource => this.DoesResourceMatchFilterParameters(resource, resourceFilterParameters)
                                                && !this.IsResourceAlreadyInUse(possibleReservationInInterval, resource));

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

        private bool IsResourceAlreadyInUse(IEnumerable<Reservation> reservationsInSameInterval, Resource resource)
        {
            return reservationsInSameInterval.Any(reservation => reservation.ResourceId == resource.Id);
        }
    }
}
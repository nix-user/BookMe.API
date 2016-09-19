using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers;
using BookMe.ShareProint.Data.Parsers.Abstract;
using BookMe.ShareProint.Data.Services.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services.Concrete
{
    public class ResourceService : BaseService, ISharePointResourceService
    {
        public ResourceService(IConverter<IDictionary<string, object>, Resource> resourceConverter,
            IConverter<IDictionary<string, object>, Reservation> reservationConverter,
            IResourceParser resourceParser,
            IReservationParser reservationParser) : base(resourceConverter, reservationConverter, resourceParser, reservationParser)
        {
        }

        private static readonly IEnumerable<string> UnallowedResources = new List<string>()
        {
            "709",
            "710",
            "712a",
            "713",
            "Netatmo",
            "NetatmoCityHall",
            "Projector 1",
            "Projector 2"
        };

        public OperationResult<IEnumerable<ResourceDTO>> GetAll()
        {
            var resourcesRetrieval = this.GetAllResources();
            return new OperationResult<IEnumerable<ResourceDTO>>()
            {
                IsSuccessful = resourcesRetrieval.IsSuccessful,
                Result = resourcesRetrieval.Result?.Select(Mapper.Map<Resource, ResourceDTO>)
            };
        }

        public OperationResult<IEnumerable<ResourceDTO>> GetAvailbleResources(ResourceFilterParameters resourceFilterParameters)
        {
            var resourcesRetrieval = this.GetAllResources();
            if (!resourcesRetrieval.IsSuccessful)
            {
                return new OperationResult<IEnumerable<ResourceDTO>>() { IsSuccessful = false };
            }

            var currentRoutineWatch = System.Diagnostics.Stopwatch.StartNew();
            var possibleReservationInIntervalRetrieval = this.GetPossibleReservationsInIntervalFromParser(new Interval(resourceFilterParameters.From, resourceFilterParameters.To));
            currentRoutineWatch.Stop();
            var elapsedMs = currentRoutineWatch.ElapsedMilliseconds;

            var filteredResources = resourcesRetrieval.Result.Where(r => !UnallowedResources.Contains(r.Title)).Select(Mapper.Map<Resource, ResourceDTO>).ToList();
            var optimizedRoutineWatch = System.Diagnostics.Stopwatch.StartNew();
            var optimizedPossibleReservationInIntervalRetrieval =
                this.GetPossibleReservationsInIntervalFromParserOptimized(new Interval(resourceFilterParameters.From, resourceFilterParameters.To), filteredResources);
            optimizedRoutineWatch.Stop();
            var elapsedMsOptimized = optimizedRoutineWatch.ElapsedMilliseconds;

            if (!possibleReservationInIntervalRetrieval.IsSuccessful && !optimizedPossibleReservationInIntervalRetrieval.IsSuccessful)
            {
                return new OperationResult<IEnumerable<ResourceDTO>>() { IsSuccessful = false };
            }

            var availableResources = resourcesRetrieval.Result.Where(resource => this.DoesResourceMatchFilterParameters(resource, resourceFilterParameters)
                                                && !this.IsResourceAlreadyInUse(possibleReservationInIntervalRetrieval.Result, resource));

            return new OperationResult<IEnumerable<ResourceDTO>>()
            {
                IsSuccessful = true,
                Result = availableResources.Select(Mapper.Map<Resource, ResourceDTO>)
            };
        }

        public OperationResult<IEnumerable<ReservationDTO>> GetRoomReservations(IntervalDTO interval, int roomId)
        {
            var reservationsRetrieval = this.GetPossibleReservationsInIntervalFromParser(Mapper.Map<IntervalDTO, Interval>(interval), roomId);
            var reservationsMapping = this.DeeplyMapReservationsToReservationDTOs(reservationsRetrieval.Result.ToList());

            return new OperationResult<IEnumerable<ReservationDTO>>()
            {
                IsSuccessful = reservationsRetrieval.IsSuccessful && reservationsMapping.IsSuccessful,
                Result = reservationsMapping.Result
            };
        }

        private bool DoesResourceMatchFilterParameters(Resource resource, ResourceFilterParameters filterParameters)
        {
            if (filterParameters.HasPolycom && !resource.HasPolycom)
            {
                return false;
            }

            if (filterParameters.RoomSize.HasValue &&
                (!resource.RoomSize.HasValue || Mapper.Map<RoomSizeDTO, RoomSize>(filterParameters.RoomSize.Value) != resource.RoomSize.Value))
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
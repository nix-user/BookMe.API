using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Controllers
{
    public class RoomController : ApiController
    {
        private ISharePointResourceService sharePointResourcesService;
        private IResourceService resourceService;

        public RoomController(ISharePointResourceService sharePointResourcesService, IResourceService resourceService)
        {
            this.sharePointResourcesService = sharePointResourcesService;
            this.resourceService = resourceService;
        }

        public ResponseModel<IEnumerable<Room>> Get()
        {
            var operationResult = this.sharePointResourcesService.GetAll();
            return new ResponseModel<IEnumerable<Room>>
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ResourceDTO, Room>)
            };
        }

        public ResponseModel<Room> Get(int id)
        {
            var operationResult = this.sharePointResourcesService.GetAll();
            if (operationResult.IsSuccessful)
            {
                var neededResource = operationResult.Result.FirstOrDefault(resource => resource.Id == id);
                return new ResponseModel<Room>()
                {
                    IsOperationSuccessful = true,
                    Result = Mapper.Map<ResourceDTO, Room>(neededResource)
                };
            }

            return new ResponseModel<Room>() { IsOperationSuccessful = false };
        }

        [Route("api/room/available")]
        [HttpGet]
        public ResponseModel<IEnumerable<Room>> GetAvailableRooms([FromUri]RoomFilterParameters filterParameters)
        {
            var mappedFilterParameters = Mapper.Map<RoomFilterParameters, ResourceFilterParameters>(filterParameters);
            var operationResult = this.resourceService.GetAvailableResources(mappedFilterParameters);
            return new ResponseModel<IEnumerable<Room>>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ResourceDTO, Room>)
            };
        }

        [Route("api/room/reservations")]
        [HttpGet]
        public ResponseModel<IEnumerable<ReservationModel>> GetRoomCurrentReservations([FromUri]RoomReservationsRequestModel reservationsModel)
        {
            var interval = new IntervalDTO(reservationsModel.From, reservationsModel.To);
            var operationResult = this.resourceService.GetRoomReservations(interval, reservationsModel.RoomId);
            return new ResponseModel<IEnumerable<ReservationModel>>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ReservationDTO, ReservationModel>)
            };
        }
    }
}
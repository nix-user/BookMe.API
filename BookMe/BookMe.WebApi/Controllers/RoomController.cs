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
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Controllers
{
    [Authorize]
    public class RoomController : ApiController
    {
        private ISharePointResourceService resourcesService;

        public RoomController(ISharePointResourceService resourcesService)
        {
            this.resourcesService = resourcesService;
        }

        public ResponseModel<IEnumerable<Room>> Get()
        {
            var operationResult = this.resourcesService.GetAll();
            return new ResponseModel<IEnumerable<Room>>
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ResourceDTO, Room>)
            };
        }

        public ResponseModel<Room> Get(int id)
        {
            var operationResult = this.resourcesService.GetAll();
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
        [HttpPost]
        public ResponseModel<IEnumerable<Room>> GetAvailableRooms([FromBody]RoomFilterParameters filterParameters)
        {
            var operationResult = this.resourcesService.GetAvailbleResources(Mapper.Map<RoomFilterParameters, ResourceFilterParameters>(filterParameters));
            return new ResponseModel<IEnumerable<Room>>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ResourceDTO, Room>)
            };
        }

        [Route("api/room/reservations")]
        [HttpPost]
        public ResponseModel<IEnumerable<ReservationModel>> GetRoomCurrentReservations(RoomReservationsRequestModel reservationsModel)
        {
            var operationResult = this.resourcesService.GetRoomReservations(new IntervalDTO(reservationsModel.From, reservationsModel.To), reservationsModel.RoomId);
            return new ResponseModel<IEnumerable<ReservationModel>>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ReservationDTO, ReservationModel>)
            };
        }
    }
}
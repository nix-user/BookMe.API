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
    public class RoomController : ApiController
    {
        private ISharePointResourceService resourcesService;

        public RoomController(ISharePointResourceService resourcesService)
        {
            this.resourcesService = resourcesService;
        }

        public IEnumerable<Room> Get()
        {
            var operationResult = this.resourcesService.GetAll();
            if (operationResult.IsSuccessful)
            {
                return operationResult.Result.Select(Mapper.Map<ResourceDTO, Room>);
            }

            return null;
        }

        public Room Get(int id)
        {
            var operationResult = this.resourcesService.GetAll();
            if (operationResult.IsSuccessful)
            {
                var neededResource = operationResult.Result.FirstOrDefault(resource => resource.Id == id);
                return Mapper.Map<ResourceDTO, Room>(neededResource);
            }

            return null;
        }

        public void Post([FromBody]string value)
        {
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }

        [Route("api/room")]
        [HttpPost]
        public IEnumerable<Room> GetAvailableRooms([FromBody]RoomFilterParameters filterParameters)
        {
            var operationResult = this.resourcesService.GetAvailbleResources(Mapper.Map<RoomFilterParameters, ResourceFilterParameters>(filterParameters));
            if (operationResult.IsSuccessful)
            {
                return operationResult.Result.Select(Mapper.Map<ResourceDTO, Room>);
            }

            return null;
        }
    }
}
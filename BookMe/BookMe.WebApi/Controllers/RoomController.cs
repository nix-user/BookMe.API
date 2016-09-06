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
            IEnumerable<ResourceDTO> allResources = this.resourcesService.GetAll();
            return allResources.Select(Mapper.Map<ResourceDTO, Room>);
        }

        public Room Get(int id)
        {
            var neededResource = this.resourcesService.GetAll().FirstOrDefault(resource => resource.Id == id);
            return Mapper.Map<ResourceDTO, Room>(neededResource);
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
    }
}
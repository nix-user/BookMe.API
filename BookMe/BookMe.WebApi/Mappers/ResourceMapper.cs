using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Mappers
{
    public static class ResourceMapper
    {
        public static Room MapResourceToRoom(Resource resource)
        {
            if (resource == null)
            {
                return null;
            }

            return new Room()
            {
                Id = resource.Id,
                IsBig = resource.RoomSize == RoomSize.Large,
                Number = resource.Title,
                IsHasPolykom = resource.HasPolycom
            };
        }
    }
}
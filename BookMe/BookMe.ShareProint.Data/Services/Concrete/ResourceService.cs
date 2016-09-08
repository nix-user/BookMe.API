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
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services.Concrete
{
    public class ResourceService : ISharePointResourceService
    {
        private IConverter<IDictionary<string, object>, Resource> resourceConverter;
        private IResourceParser resourceParser;

        public ResourceService(IConverter<IDictionary<string, object>, Resource> resourceConverter, IResourceParser resourceParser)
        {
            this.resourceConverter = resourceConverter;
            this.resourceParser = resourceParser;
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
            return new OperationResult<IEnumerable<ResourceDTO>>()
            {
                IsSuccessful = true,
                Result = new List<ResourceDTO>()
            };
        }

        public OperationResult<IEnumerable<ReservationDTO>> GetRoomReservations(DateTime intervalStart, DateTime intervalEnd, int roomId)
        {
            return new OperationResult<IEnumerable<ReservationDTO>>()
            {
                IsSuccessful = true,
                Result = new List<ReservationDTO>()
            };
        }
    }
}
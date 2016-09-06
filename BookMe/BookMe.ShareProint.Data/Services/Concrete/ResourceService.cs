using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
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

        public IEnumerable<ResourceDTO> GetAll()
        {
            var resourceDictionariesCollection = this.resourceParser.GetAll().ToList().Select(x => x.FieldValues);
            return this.resourceConverter.Convert(resourceDictionariesCollection).Select(Mapper.Map<Resource, ResourceDTO>);
        }
    }
}
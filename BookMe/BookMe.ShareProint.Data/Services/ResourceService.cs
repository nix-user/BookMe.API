using System.Collections.Generic;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Services
{
    public class ResourceService : ISharePointResourceService
    {
        private IConverter<ListItem, Resource> resourceConverter;
        private IResourceParser resourceParser;

        public ResourceService(IConverter<ListItem, Resource> resourceConverter, IResourceParser resourceParser)
        {
            this.resourceConverter = resourceConverter;
            this.resourceParser = resourceParser;
        }

        public IEnumerable<Resource> GetAll()
        {
            var resourceCollection = this.resourceParser.GetAll();
            return this.resourceConverter.Convert(resourceCollection);
        }
    }
}
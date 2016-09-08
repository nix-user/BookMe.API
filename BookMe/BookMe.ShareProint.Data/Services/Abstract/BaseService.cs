using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Parsers;
using BookMe.ShareProint.Data.Parsers.Abstract;

namespace BookMe.ShareProint.Data.Services.Abstract
{
    public abstract class BaseService
    {
        private readonly IResourceParser resourceParser;
        private IConverter<IDictionary<string, object>, Resource> resourcesConverter;

        protected BaseService(IResourceParser resourceParser, IConverter<IDictionary<string, object>, Resource> resourcesConverter)
        {
            this.resourceParser = resourceParser;
            this.resourcesConverter = resourcesConverter;
        }

        protected IEnumerable<Resource> GetAllResources(out bool isSuccessful)
        {
            try
            {
                var resourceDictionariesCollection = this.resourceParser.GetAll().ToList().Select(x => x.FieldValues);
                isSuccessful = true;
                return this.resourcesConverter.Convert(resourceDictionariesCollection);
            }
            catch (ParserException)
            {
                isSuccessful = false;
                return null;
            }
        }
    }
}

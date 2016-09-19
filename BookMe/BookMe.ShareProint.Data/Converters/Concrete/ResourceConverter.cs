using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Constants;
using BookMe.ShareProint.Data.Converters.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Converters.Concrete
{
    public class ResourceConverter : IConverter<IDictionary<string, object>, Resource>
    {
        private readonly IDescriptionParser descriptionParser;

        public ResourceConverter(IDescriptionParser descriptionParser)
        {
            this.descriptionParser = descriptionParser;
        }

        public Resource Convert(IDictionary<string, object> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var decription = value[FieldNames.DescriptionKey]?.ToString();

            var resource = new Resource()
            {
                Id = int.Parse(value[FieldNames.IdKey].ToString()),
                Title = value[FieldNames.TitleKey].ToString(),
                Description = decription,
                HasPolycom = this.descriptionParser.HasPolycom(decription),
                HasTv = this.descriptionParser.HasTv(decription),
                RoomSize = this.descriptionParser.ParseRoomSize(decription)
            };

            return resource;
        }

        public IEnumerable<Resource> Convert(IEnumerable<IDictionary<string, object>> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(this.Convert);
        }

        public IDictionary<string, object> ConvertBack(Resource value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDictionary<string, object>> ConvertBack(IEnumerable<Resource> values)
        {
            throw new NotImplementedException();
        }
    }
}
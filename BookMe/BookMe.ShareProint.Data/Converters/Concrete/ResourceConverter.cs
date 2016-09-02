using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Converters.Concrete
{
    class ResourceConverter : IConverter<ListItem, Resource>
    {
        private const string IdKey = "ID";
        private const string TitleKey = "Title";
        private const string DescriptionKey = "Description";

        private readonly IDescriptionParser descriptionParser;

        public ResourceConverter(IDescriptionParser descriptionParser)
        {
            this.descriptionParser = descriptionParser;
        }

        public Resource Convert(ListItem value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var decription = value.FieldValues[DescriptionKey]?.ToString();

            var resource = new Resource()
            {
                Id = int.Parse(value.FieldValues[IdKey].ToString()),
                Title = value.FieldValues[TitleKey].ToString(),
                Description = decription,
                HasPolycom = this.descriptionParser.HasPolycom(decription),
                HasTv = this.descriptionParser.HasTv(decription),
                RoomSize = this.descriptionParser.ParseRoomSize(decription)
            };

            return resource;
        }

        public IEnumerable<Resource> Convert(IEnumerable<ListItem> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(this.Convert);
        }
    }
}

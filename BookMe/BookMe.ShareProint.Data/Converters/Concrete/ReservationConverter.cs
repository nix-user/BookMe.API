using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Converters.Concrete
{
    public class ReservationConverter : IConverter<IDictionary<string, object>, Reservation>
    {
        private const string IdKey = "ID";
        private const string TitleKey = "Title";
        private const string DescriptionKey = "Description";
        private const string FacilitiesKey = "Facilities";
        private const string EventDateKey = "EventDate";
        private const string EndDateKey = "EndDate";
        private const string DurationKey = "Duration";
        private const string IsRecurrenceKey = "fRecurrence";
        private const string AuthorKey = "Author";

        public Reservation Convert(IDictionary<string, object> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var reservation = new Reservation()
            {
                Id = int.Parse(value[IdKey].ToString()),
                Title = value[TitleKey].ToString(),
                Description = value[DescriptionKey]?.ToString(),
                ResourceId = (value[FacilitiesKey] as IList<FieldLookupValue>)?[0].LookupId,
                EventDate = (DateTime)value[EventDateKey],
                EndDate = (DateTime)value[EndDateKey],
                Duration = TimeSpan.FromSeconds(long.Parse(value[DurationKey].ToString())),
                IsRecurrence = bool.Parse(value[IsRecurrenceKey].ToString()),
                OwnerName = (value[AuthorKey] as FieldUserValue)?.LookupValue
            };

            return reservation;
        }

        public IEnumerable<Reservation> Convert(IEnumerable<IDictionary<string, object>> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(this.Convert);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
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
        private const string ParrentIdKey = "MasterSeriesItemID";
        private const string EventTypeKey = "EventType";
        private const string IsAllDayEventKey = "fAllDayEvent";

        private readonly IConverter<IDictionary<string, object>, RecurrenceData> recurrenceDataConverter;

        public ReservationConverter(IConverter<IDictionary<string, object>, RecurrenceData> recurrenceDataConverter)
        {
            this.recurrenceDataConverter = recurrenceDataConverter;
        }

        public Reservation Convert(IDictionary<string, object> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            int? parrentIdValue = null;
            var parrentId = value[ParrentIdKey];
            if (parrentId != null)
            {
                parrentIdValue = int.Parse(parrentId.ToString());
            }

            var reservation = new Reservation
            {
                Id = int.Parse(value[IdKey].ToString()),
                Title = value[TitleKey].ToString(),
                Description = value[DescriptionKey]?.ToString(),
                ResourceId = (value[FacilitiesKey] as IList<FieldLookupValue>)?[0].LookupId,
                EventDate = (DateTime)value[EventDateKey] + TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
                EndDate = (DateTime)value[EndDateKey] + TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
                Duration = TimeSpan.FromSeconds(long.Parse(value[DurationKey].ToString())),
                IsRecurrence = bool.Parse(value[IsRecurrenceKey].ToString()),
                OwnerName = (value[AuthorKey] as FieldUserValue)?.LookupValue,
                RecurrenceData = this.recurrenceDataConverter.Convert(value),
                EventType = int.Parse(value[EventTypeKey].ToString()),
                ParentId = parrentIdValue,
                IsAllDayEvent = bool.Parse(value[IsAllDayEventKey].ToString())
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
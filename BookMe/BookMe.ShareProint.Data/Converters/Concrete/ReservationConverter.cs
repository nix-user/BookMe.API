using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Constants;
using BookMe.ShareProint.Data.Converters.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Converters.Concrete
{
    public class ReservationConverter : IConverter<IDictionary<string, object>, Reservation>
    {
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
            var parrentId = value[FieldNames.ParrentIdKey];
            if (parrentId != null)
            {
                parrentIdValue = int.Parse(parrentId.ToString());
            }

            var reservation = new Reservation
            {
                Id = int.Parse(value[FieldNames.IdKey].ToString()),
                Title = value[FieldNames.TitleKey].ToString(),
                Description = value[FieldNames.DescriptionKey]?.ToString(),
                ResourceId = (value[FieldNames.FacilitiesKey] as IList<FieldLookupValue>)?[0].LookupId,
                EventDate = (DateTime)value[FieldNames.EventDateKey],
                EndDate = (DateTime)value[FieldNames.EndDateKey],
                Duration = TimeSpan.FromSeconds(long.Parse(value[FieldNames.DurationKey].ToString())),
                IsRecurrence = bool.Parse(value[FieldNames.IsRecurrenceKey].ToString()),
                OwnerName = (value[FieldNames.AuthorKey] as FieldUserValue)?.LookupValue,
                RecurrenceData = this.recurrenceDataConverter.Convert(value),
                RecurrenceDate = (value[FieldNames.RecurrenceDate] as DateTime?),
                EventType = (EventType)int.Parse(value[FieldNames.EventTypeKey].ToString()),
                ParentId = parrentIdValue,
                IsAllDayEvent = bool.Parse(value[FieldNames.IsAllDayEventKey].ToString())
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

        public IDictionary<string, object> ConvertBack(Reservation value)
        {
            return new Dictionary<string, object>()
            {
                { FieldNames.TitleKey, value.Title },
                { FieldNames.DescriptionKey, value.Description },
                { FieldNames.FacilitiesKey, new FieldLookupValue() { LookupId = value.ResourceId.Value } },
                { FieldNames.EventDateKey, value.EventDate },
                { FieldNames.EndDateKey, value.EndDate },
                { FieldNames.DurationKey, value.Duration.Seconds },
                { FieldNames.IsAllDayEventKey, value.IsAllDayEvent }
            };
        }

        public IEnumerable<IDictionary<string, object>> ConvertBack(IEnumerable<Reservation> values)
        {
            return values.Select(this.ConvertBack);
        }
    }
}
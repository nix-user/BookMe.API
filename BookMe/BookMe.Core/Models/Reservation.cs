using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;
using BookMe.Core.Models.Recurrence;

namespace BookMe.Core.Models
{
    public class Reservation : BaseEntity
    {
        private DateTime eventDate;

        private DateTime endDate;

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime EventDate
        {
            get { return this.eventDate; }
            set
            {
                this.eventDate = value.Add(TimeZoneInfo.Local.GetUtcOffset(value) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
            }
        }

        public DateTime EndDate
        {
            get { return this.endDate; }
            set
            {
                this.endDate = value.Add(TimeZoneInfo.Local.GetUtcOffset(value) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
            }
        }

        public DateTime? RecurrenceDate { get; set; }

        public bool IsRecurrence { get; set; }

        public int? ResourceId { get; set; }

        public TimeSpan Duration { get; set; }

        public string OwnerName { get; set; }

        public RecurrenceData RecurrenceData { get; set; }

        public int? ParentId { get; set; }

        public EventType EventType { get; set; }

        public bool IsAllDayEvent { get; set; }

        public Interval GetBusyInterval(DateTime date)
        {
            if (date > this.EndDate)
            {
                return null;
            }

            if (this.RecurrenceData == null)
            {
                return new Interval(this.EventDate, this.EndDate);
            }

            var isBusy = this.RecurrenceData.IsBusyInDate(date);
            if (!isBusy)
            {
                return null;
            }

            var startDate = new DateTime(date.Year, date.Month, date.Day, this.EventDate.Hour, this.EventDate.Minute, this.EventDate.Second);
            var endDate = new DateTime(date.Year, date.Month, date.Day, this.EndDate.Hour, this.EndDate.Minute, this.EndDate.Second);

            return new Interval(startDate, endDate);
        }

        public override string ToString()
        {
            const string TimeFormat = "M/dd/yy HH:mm";
            var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(this.EventDate);

            var result = string.Empty;

            if (this.IsAllDayEvent)
            {
                result += "Весь день";
            }
            else
            {
                result += $"{this.EventDate.Add(timeZoneOffset).ToString(TimeFormat)} - {this.EndDate.Add(timeZoneOffset).ToString(TimeFormat)}";
            }

            return result;
        }
    }
}
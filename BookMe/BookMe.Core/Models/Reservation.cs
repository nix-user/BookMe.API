using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models.Recurrence;

namespace BookMe.Core.Models
{
    public class Reservation : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsRecurrence { get; set; }

        public int? ResourceId { get; set; }

        public TimeSpan Duration { get; set; }

        public string OwnerName { get; set; }

        public RecurrenceData RecurrenceData { get; set; }

        public int? ParentId { get; set; }

        public int EventType { get; set; }

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
    }
}
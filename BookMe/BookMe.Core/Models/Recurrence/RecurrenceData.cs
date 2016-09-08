using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public abstract class RecurrenceData
    {
        protected IDictionary<DayOfTheWeek, IEnumerable<DayOfWeek>> DaysOfWeekByDayOfTheWeek { get; set; } = new Dictionary<DayOfTheWeek, IEnumerable<DayOfWeek>>()
        {
            { DayOfTheWeek.Sunday, new List<DayOfWeek>() { DayOfWeek.Sunday } },
            { DayOfTheWeek.Monday, new List<DayOfWeek>() { DayOfWeek.Monday } },
            { DayOfTheWeek.Tuesday, new List<DayOfWeek>() { DayOfWeek.Tuesday } },
            { DayOfTheWeek.Wednesday, new List<DayOfWeek>() { DayOfWeek.Wednesday } },
            { DayOfTheWeek.Thursday, new List<DayOfWeek>() { DayOfWeek.Thursday } },
            { DayOfTheWeek.Friday, new List<DayOfWeek>() { DayOfWeek.Friday } },
            { DayOfTheWeek.Saturday, new List<DayOfWeek>() { DayOfWeek.Saturday } },
            {
                DayOfTheWeek.Day, new List<DayOfWeek>()
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                }
            },
            {
                DayOfTheWeek.Weekday, new List<DayOfWeek>()
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                }
            },
            { DayOfTheWeek.Sunday, new List<DayOfWeek>() { DayOfWeek.Saturday, DayOfWeek.Sunday } },
        };

        public DateTime? EndDate { get; set; }

        public int? NumberOfOccurrences { get; set; }

        public DateTime StartDate { get; set; }

        public int? Interval { get; set; }

        public abstract bool IsBusyInDate(DateTime date);
    }
}

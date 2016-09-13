using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public abstract class RecurrenceData
    {
        protected IDictionary<DayOfWeek, IEnumerable<DayOfTheWeek>> DaysOThefWeekByDayOfWeek { get; set; } = new Dictionary<DayOfWeek, IEnumerable<DayOfTheWeek>>()
        {
            { DayOfWeek.Sunday, new List<DayOfTheWeek>() { DayOfTheWeek.Sunday, DayOfTheWeek.Day, DayOfTheWeek.WeekendDay } },
            { DayOfWeek.Monday, new List<DayOfTheWeek>() { DayOfTheWeek.Monday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Tuesday, new List<DayOfTheWeek>() { DayOfTheWeek.Tuesday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Wednesday, new List<DayOfTheWeek>() { DayOfTheWeek.Wednesday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Thursday, new List<DayOfTheWeek>() { DayOfTheWeek.Thursday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Friday, new List<DayOfTheWeek>() { DayOfTheWeek.Friday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Saturday, new List<DayOfTheWeek>() { DayOfTheWeek.Saturday, DayOfTheWeek.Day, DayOfTheWeek.WeekendDay } },
        };

        public DateTime? EndDate { get; set; }

        public int? NumberOfOccurrences { get; set; }

        public DateTime StartDate { get; set; }

        public int? Interval { get; set; }

        public abstract bool IsBusyInDate(DateTime date);

        protected bool IsDateInDaysOfTheWeek(DateTime date, IEnumerable<DayOfTheWeek> daysOfTheWeek)
        {
            foreach (var item in daysOfTheWeek)
            {
                if (this.DaysOThefWeekByDayOfWeek[date.DayOfWeek].Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        protected IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
    }
}
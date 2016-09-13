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

        public virtual bool IsBusyInDate(DateTime date)
        {
            var totalPeriodsCount = this.CalculatePeriodsCount(date);
            var doesMatchDateCondition = this.DoesMatchDateCondition(date);

            if (totalPeriodsCount % this.Interval != 0)
            {
                return false;
            }
            else
            {
                if (this.NumberOfOccurrences != null)
                {
                    var countOfInstances = this.CalculateInstancesCount(date);
                    if (countOfInstances <= this.NumberOfOccurrences)
                    {
                        return doesMatchDateCondition;
                    }

                    return false;
                }
            }

            return (this.EndDate == null || this.EndDate > date) && doesMatchDateCondition;
        }

        protected virtual int CalculateInstancesCount(DateTime to)
        {
            var days = this.EachDay(this.StartDate, to).ToList();
            var yearsCount = 0;
            var countOfInstances = 0;
            for (var i = 0; i < days.Count; i++)
            {
                if (this.IsNextInterval(days, i))
                {
                    yearsCount++;
                }

                if (yearsCount % this.Interval == 0)
                {
                    countOfInstances += this.DoesMatchDateCondition(days[i]) ? 1 : 0;
                }
            }

            return countOfInstances;
        }

        protected abstract int CalculatePeriodsCount(DateTime to);

        protected abstract bool DoesMatchDateCondition(DateTime date);

        protected abstract bool IsNextInterval(IList<DateTime> days, int index);

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

        protected int CalculateMonthCount(DateTime from, DateTime to)
        {
            return (to.Month - from.Month) + 12 * (to.Year - from.Year);
        }

        protected IEnumerable<WeekRange> GetRange(int year, int month)
        {
            DateTime start = new DateTime(year, month, 1).AddDays(-6);
            DateTime end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Monday)
                {
                    yield return new WeekRange(date);
                }
            }
        }
    }
}

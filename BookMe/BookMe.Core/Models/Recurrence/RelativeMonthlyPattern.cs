using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class RelativeMonthlyPattern : RecurrenceData
    {
        public IEnumerable<DayOfTheWeek> DaysOfTheWeek { get; set; }

        public DayOfTheWeekIndex DayOfTheWeekIndex { get; set; }

        public override bool IsBusyInDate(DateTime date)
        {
            var totalMonth = this.CalculateMonthCount(this.StartDate, date);
            var isDayOfMonthRight = this.DoesMatchDateCondition(date);
            if (totalMonth % this.Interval != 0)
            {
                return false;
            }
            else
            {
                if (this.NumberOfOccurrences != null)
                {
                    var countOfInstances = this.CalculateInstancesCount(this.StartDate, date);
                    if (countOfInstances <= this.NumberOfOccurrences)
                    {
                        return isDayOfMonthRight;
                    }

                    return false;
                }
            }

            return (this.EndDate == null || this.EndDate > date) && isDayOfMonthRight;
        }

        private int CalculateInstancesCount(DateTime from, DateTime to)
        {
            var days = this.EachDay(from, to).ToList();
            var monthsCount = 0;
            var countOfInstances = 0;
            for (var i = 0; i < days.Count; i++)
            {
                if (days[i].Day == 1 && i != 0)
                {
                    monthsCount++;
                }

                if (monthsCount % this.Interval == 0)
                {
                    countOfInstances += this.DoesMatchDateCondition(days[i]) ? 1 : 0;
                }
            }

            return countOfInstances;
        }

        private WeekRange GetNeededWeek(int firstWeekHasWeekdayIndex, int firstDayHasNotWeekdayIndex, IList<WeekRange> weeks, DateTime date)
        {
            var firstWeek = weeks[0];
            for (var d = firstWeek.Start; d.Date <= firstWeek.End; d = d.AddDays(1))
            {
                if (d.Month == date.Month && this.IsDateInDaysOfTheWeek(d, this.DaysOfTheWeek))
                {
                    return weeks[firstWeekHasWeekdayIndex];
                }
            }

            return weeks[firstDayHasNotWeekdayIndex];
        }

        public bool DoesMatchDateCondition(DateTime date)
        {
            var weeks = this.GetRange(date.Year, date.Month).ToList();
            WeekRange neededWeek = null;
            switch (this.DayOfTheWeekIndex)
            {
                case DayOfTheWeekIndex.First:
                    neededWeek = this.GetNeededWeek(0, 1, weeks, date);
                    break;
                case DayOfTheWeekIndex.Second:
                    neededWeek = this.GetNeededWeek(1, 2, weeks, date);
                    break;
                case DayOfTheWeekIndex.Third:
                    neededWeek = this.GetNeededWeek(2, 3, weeks, date);
                    break;
                case DayOfTheWeekIndex.Fourth:
                    neededWeek = this.GetNeededWeek(3, 4, weeks, date);
                    break;
                case DayOfTheWeekIndex.Last:
                    var weekCount = weeks.Count;
                    var lastWeekIndex = weekCount - 1;
                    var weekBeforeLastIndex = weekCount - 2;
                    neededWeek = this.GetNeededWeek(lastWeekIndex, weekBeforeLastIndex, weeks, date);
                    break;
            }

            for (DateTime d = neededWeek.Start; d.Date <= neededWeek.End; d = d.AddDays(1))
            {
                if (d.Day == date.Day)
                {
                    if (this.IsDateInDaysOfTheWeek(d, this.DaysOfTheWeek))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }
    }
}

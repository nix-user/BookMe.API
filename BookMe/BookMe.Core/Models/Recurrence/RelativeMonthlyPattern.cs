using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class RelativeMonthlyPattern : RelativePattern
    {
        protected override int CalculatePeriodsCount(DateTime to)
        {
            return this.CalculateMonthCount(this.StartDate, to);
        }

        protected override bool DoesMatchDateCondition(DateTime date)
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

        protected override bool IsNextInterval(IList<DateTime> days, int index)
        {
            return days[index].Day == 1 && index != 0;
        }
    }
}

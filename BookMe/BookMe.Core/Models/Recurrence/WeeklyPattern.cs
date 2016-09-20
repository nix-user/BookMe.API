using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class WeeklyPattern : CommonPattern
    {
        public IEnumerable<DayOfTheWeek> DaysOfTheWeek { get; set; }

        protected override int CalculatePeriodsCount(DateTime to)
        {
            const DayOfWeek FirstDayOfWeek = DayOfWeek.Monday;
            const DayOfWeek LastDayOfWeek = DayOfWeek.Sunday;
            const int DaysInWeek = 7;

            DateTime firstDayOfWeekBeforeStartDate;
            var daysBetweenStartDateAndPreviousFirstDayOfWeek = (int)this.StartDate.DayOfWeek - (int)FirstDayOfWeek;
            if (daysBetweenStartDateAndPreviousFirstDayOfWeek >= 0)
            {
                firstDayOfWeekBeforeStartDate = this.StartDate.AddDays(-daysBetweenStartDateAndPreviousFirstDayOfWeek);
            }
            else
            {
                firstDayOfWeekBeforeStartDate = this.StartDate.AddDays(-(daysBetweenStartDateAndPreviousFirstDayOfWeek + DaysInWeek));
            }

            DateTime lastDayOfWeekAfterEndDate;
            var daysBetweenEndDateAndFollowingLastDayOfWeek = (int)LastDayOfWeek - (int)to.DayOfWeek;
            if (daysBetweenEndDateAndFollowingLastDayOfWeek >= 0)
            {
                lastDayOfWeekAfterEndDate = to.AddDays(daysBetweenEndDateAndFollowingLastDayOfWeek);
            }
            else
            {
                lastDayOfWeekAfterEndDate = to.AddDays(daysBetweenEndDateAndFollowingLastDayOfWeek + DaysInWeek);
            }

            var calendarWeeks = (int)((lastDayOfWeekAfterEndDate - firstDayOfWeekBeforeStartDate).TotalDays / DaysInWeek);
            return calendarWeeks;
        }

        protected override bool DoesMatchDateCondition(DateTime date)
        {
            return this.IsDateInDaysOfTheWeek(date, this.DaysOfTheWeek);
        }

        protected override bool IsNextInterval(IList<DateTime> days, int index)
        {
            return days[index].DayOfWeek == DayOfWeek.Monday && index != 0;
        }

        public override string ToString()
        {
            return $"Каждую {this.Interval} неделю каждый {DaysOfWeekToString(this.DaysOfTheWeek)}.";
        }
    }
}
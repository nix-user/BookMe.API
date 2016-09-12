using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class WeeklyPattern : RecurrenceData
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

        protected override int CalculateInstancesCount(DateTime to)
        {
            var days = this.EachDay(this.StartDate, to).ToList();
            var weekCount = 0;
            var countOfInstances = 0;
            for (var i = 0; i < days.Count; i++)
            {
                if (days[i].DayOfWeek == DayOfWeek.Monday && i != 0)
                {
                    weekCount++;
                }

                if (weekCount % this.Interval == 0)
                {
                    countOfInstances += this.IsDateInDaysOfTheWeek(days[i], this.DaysOfTheWeek) ? 1 : 0;
                }
            }

            return countOfInstances;
        }

        protected override bool DoesMatchDateCondition(DateTime date)
        {
            return this.IsDateInDaysOfTheWeek(date, this.DaysOfTheWeek);
        }
    }
}

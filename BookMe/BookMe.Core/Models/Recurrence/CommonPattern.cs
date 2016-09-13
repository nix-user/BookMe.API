using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public abstract class CommonPattern : RecurrenceData
    {
        public override bool IsBusyInDate(DateTime date)
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
            var intervalsCount = 0;
            var countOfInstances = 0;
            for (var i = 0; i < days.Count; i++)
            {
                if (this.IsNextInterval(days, i))
                {
                    intervalsCount++;
                }

                if (intervalsCount % this.Interval == 0)
                {
                    countOfInstances += this.DoesMatchDateCondition(days[i]) ? 1 : 0;
                }
            }

            return countOfInstances;
        }

        protected abstract int CalculatePeriodsCount(DateTime to);

        protected abstract bool DoesMatchDateCondition(DateTime date);

        protected abstract bool IsNextInterval(IList<DateTime> days, int index);

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

        protected int CalculateMonthCount(DateTime from, DateTime to)
        {
            return (to.Month - from.Month) + 12 * (to.Year - from.Year);
        }
    }
}

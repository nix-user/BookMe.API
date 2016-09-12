using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class MonthlyPattern : RecurrenceData
    {
        public int DayOfMonth { get; set; }

        protected override int CalculatePeriodsCount(DateTime to)
        {
            return this.CalculateMonthCount(this.StartDate, to);
        }

        protected override int CalculateInstancesCount(DateTime to)
        {
            var days = this.EachDay(this.StartDate, to).ToList();
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
                    countOfInstances += this.DayOfMonth == days[i].Day ? 1 : 0;
                }
            }

            return countOfInstances;
        }

        protected override bool DoesMatchDateCondition(DateTime date)
        {
            return this.DayOfMonth == date.Day;
        }
    }
}

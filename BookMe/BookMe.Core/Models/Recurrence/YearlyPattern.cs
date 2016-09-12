using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class YearlyPattern : RecurrenceData
    {
        public int DayOfMonth { get; set; }

        public Month Month { get; set; }

        protected override int CalculatePeriodsCount(DateTime to)
        {
            return to.Year - this.StartDate.Year;
        }

        protected override int CalculateInstancesCount(DateTime to)
        {
            var days = this.EachDay(this.StartDate, to).ToList();
            var yearsCount = 0;
            var countOfInstances = 0;
            for (var i = 0; i < days.Count; i++)
            {
                if (days[i].Day == 1 && days[i].Month == 1 && i != 0)
                {
                    yearsCount++;
                }

                if (yearsCount % this.Interval == 0)
                {
                    countOfInstances += this.DayOfMonth == days[i].Day && (int)this.Month == days[i].Month ? 1 : 0;
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

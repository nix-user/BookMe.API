using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class DailyPattern : RecurrenceData
    {
        public IEnumerable<DayOfTheWeek> DaysOfTheWeek { get; set; }

        public override bool IsBusyInDate(DateTime date)
        {
            if (this.Interval == null)
            {
                if (this.NumberOfOccurrences != null)
                {
                    var countOfInstances = this.CalculateInstancesCount(date);
                    return countOfInstances <= this.NumberOfOccurrences;
                }
            }
            else
            {
                var totalDays = this.CalculatePeriodsCount(date);
                if (totalDays % this.Interval != 0)
                {
                    return false;
                }
                else if (this.NumberOfOccurrences != null)
                {
                    return totalDays < this.Interval * this.NumberOfOccurrences;
                }
            }

            return this.EndDate == null || this.EndDate > date;
        }

        private int CalculatePeriodsCount(DateTime to)
        {
            return (int)(to - this.StartDate).TotalDays;
        }

        private int CalculateInstancesCount(DateTime to)
        {
            return this.EachDay(this.StartDate, to)
                .Count(item => this.IsDateInDaysOfTheWeek(item, this.DaysOfTheWeek));
        }
    }
}

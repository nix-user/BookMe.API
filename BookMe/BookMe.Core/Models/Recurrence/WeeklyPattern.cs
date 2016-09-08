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

        public override bool IsBusyInDate(DateTime date)
        {
            var totalWeeks = this.CalculateWeeksCount(this.StartDate, date);
            var isDateInDaysOfTheWeek = this.IsDateInDaysOfTheWeek(date, this.DaysOfTheWeek);

            if ((totalWeeks - 1) % this.Interval != 0)
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
                        return isDateInDaysOfTheWeek;
                    }

                    return false;
                }
            }

            return (this.EndDate == null || this.EndDate > date) && isDateInDaysOfTheWeek;
        }

        private int CalculateInstancesCount(DateTime from, DateTime to)
        {
            var days = this.EachDay(from, to).ToList();
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
    }
}

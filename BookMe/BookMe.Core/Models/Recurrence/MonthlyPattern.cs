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

        public override bool IsBusyInDate(DateTime date)
        {
            var totalMonth = this.CalculateMonthCount(this.StartDate, date);
            var isDayOfMonthRight = this.DayOfMonth == date.Day;
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

        private int CalculateMonthCount(DateTime from, DateTime to)
        {
            return (to.Month - from.Month) + 12 * (to.Year - from.Year);
        }

        private int CalculateInstancesCount(DateTime from, DateTime to)
        {
            var totalMonth = this.CalculateMonthCount(from, to);
            if (from.Day > this.DayOfMonth)
            {
                totalMonth--;
            }

            if (to.Day < this.DayOfMonth)
            {
                totalMonth--;
            }

            return totalMonth;
        }
    }
}

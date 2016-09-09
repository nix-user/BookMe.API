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

        public override bool IsBusyInDate(DateTime date)
        {
            var yearsCount = date.Year - this.StartDate.Year;
            var isDayOfMonthRight = this.DayOfMonth == date.Day;
            if (yearsCount % this.Interval != 0)
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
    }
}

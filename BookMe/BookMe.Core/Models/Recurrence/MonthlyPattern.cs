﻿using System;
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

        private int CalculateInstancesCount(DateTime from, DateTime to)
        {
            var days = this.EachDay(from, to).ToList();
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
    }
}

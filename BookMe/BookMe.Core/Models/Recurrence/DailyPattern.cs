﻿using System;
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
                    var countOfInstances = this.EachDay(this.StartDate, SystemTime.Now())
                        .Count(item => this.IsDateInDaysOfTheWeek(item, this.DaysOfTheWeek));

                    return countOfInstances <= this.NumberOfOccurrences;
                }
            }
            else
            {
                var totalDays = (int)(date - this.StartDate).TotalDays;
                if (totalDays % this.Interval != 0)
                {
                    return false;
                }
                else if (this.NumberOfOccurrences != null)
                {
                    return totalDays < this.Interval * this.NumberOfOccurrences;
                }
            }

            return this.EndDate == null || this.EndDate > SystemTime.Now();
        }

        private bool IsDateInDaysOfTheWeek(DateTime date, IEnumerable<DayOfTheWeek> daysOfTheWeek)
        {
            foreach (var item in daysOfTheWeek)
            {
                if (this.DaysOThefWeekByDayOfWeek[date.DayOfWeek].Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
    }
}

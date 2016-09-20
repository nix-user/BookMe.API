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

        private static IDictionary<DayOfTheWeek, string> DayOfTheWeekToText = new Dictionary<DayOfTheWeek, string>()
        {
            { DayOfTheWeek.Day, "день" },
            { DayOfTheWeek.Weekday, "рабочий день" },
            { DayOfTheWeek.WeekendDay, "выходной" },
            { DayOfTheWeek.Monday, "ПН" },
            { DayOfTheWeek.Tuesday, "ВТ" },
            { DayOfTheWeek.Wednesday, "СР" },
            { DayOfTheWeek.Thursday, "ЧТ" },
            { DayOfTheWeek.Friday, "ПТ" },
            { DayOfTheWeek.Saturday, "СБ" },
            { DayOfTheWeek.Sunday, "ВС" },
        };

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

        public override string ToString()
        {
            var result = string.Empty;

            if (this.Interval != null)
            {
                result += $"Каждый {this.Interval} день.";
            }
            else
            {
                result += DaysOfWeekToString(this.DaysOfTheWeek);
            }

            return result;
        }

        private static string DaysOfWeekToString(IEnumerable<DayOfTheWeek> daysOfTheWeek)
        {
            const string separator = ", ";

            return string.Join(separator, daysOfTheWeek.Select(x => DayOfTheWeekToText[x]));
        }
    }
}
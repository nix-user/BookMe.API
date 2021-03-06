﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public abstract class RecurrenceData
    {
        protected static readonly IDictionary<DayOfWeek, IEnumerable<DayOfTheWeek>> DaysOfTheWeekByDayOfWeek = new Dictionary<DayOfWeek, IEnumerable<DayOfTheWeek>>()
        {
            { DayOfWeek.Sunday, new List<DayOfTheWeek>() { DayOfTheWeek.Sunday, DayOfTheWeek.Day, DayOfTheWeek.WeekendDay } },
            { DayOfWeek.Monday, new List<DayOfTheWeek>() { DayOfTheWeek.Monday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Tuesday, new List<DayOfTheWeek>() { DayOfTheWeek.Tuesday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Wednesday, new List<DayOfTheWeek>() { DayOfTheWeek.Wednesday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Thursday, new List<DayOfTheWeek>() { DayOfTheWeek.Thursday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Friday, new List<DayOfTheWeek>() { DayOfTheWeek.Friday, DayOfTheWeek.Day, DayOfTheWeek.Weekday } },
            { DayOfWeek.Saturday, new List<DayOfTheWeek>() { DayOfTheWeek.Saturday, DayOfTheWeek.Day, DayOfTheWeek.WeekendDay } },
        };

        private static readonly IDictionary<DayOfTheWeek, string> DayOfTheWeekToText = new Dictionary<DayOfTheWeek, string>()
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

        protected static readonly IDictionary<Month, string> MonthToText = new Dictionary<Month, string>()
        {
            { Month.January, "января" },
            { Month.February, "февраля" },
            { Month.March, "марта" },
            { Month.April, "апреля" },
            { Month.May, "мая" },
            { Month.June, "июня" },
            { Month.July, "июля" },
            { Month.August, "августа" },
            { Month.September, "сентября" },
            { Month.October, "октября" },
            { Month.November, "ноября" },
            { Month.December, "декабря" },
        };

        public DateTime? EndDate { get; set; }

        public int? NumberOfOccurrences { get; set; }

        public DateTime StartDate { get; set; }

        public int? Interval { get; set; }

        protected static string DaysOfWeekToString(IEnumerable<DayOfTheWeek> daysOfTheWeek)
        {
            const string Separator = ", ";

            return string.Join(Separator, daysOfTheWeek.Select(x => DayOfTheWeekToText[x]));
        }

        protected bool IsDateInDaysOfTheWeek(DateTime date, IEnumerable<DayOfTheWeek> daysOfTheWeek)
        {
            foreach (var item in daysOfTheWeek)
            {
                if (DaysOfTheWeekByDayOfWeek[date.DayOfWeek].Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        protected IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        public bool IsBusyInDate(DateTime date)
        {
            if (this.StartDate.Date > date.Date)
            {
                return false;
            }

            return this.DoesDateMatchRuleCondition(date);
        }

        protected abstract bool DoesDateMatchRuleCondition(DateTime date);
    }
}
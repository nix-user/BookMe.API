using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class RelativeMonthlyPattern : RelativePattern
    {
        protected override int CalculatePeriodsCount(DateTime to)
        {
            return this.CalculateMonthCount(this.StartDate, to);
        }

        protected override bool IsNextInterval(IList<DateTime> days, int index)
        {
            return days[index].Day == 1 && index != 0;
        }

        public override string ToString()
        {
            return $"Каждый {this.Interval} месяц, {DaysOfWeekToString(this.DaysOfTheWeek)} {DayOfTheWeekIndexToText[this.DayOfTheWeekIndex]} недели";
        }
    }
}
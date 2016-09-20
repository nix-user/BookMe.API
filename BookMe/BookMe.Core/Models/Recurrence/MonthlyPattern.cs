using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class MonthlyPattern : CommonPattern
    {
        public int DayOfMonth { get; set; }

        protected override int CalculatePeriodsCount(DateTime to)
        {
            return this.CalculateMonthCount(this.StartDate, to);
        }

        protected override bool DoesMatchDateCondition(DateTime date)
        {
            return this.DayOfMonth == date.Day;
        }

        protected override bool IsNextInterval(IList<DateTime> days, int index)
        {
            return days[index].Day == 1 && index != 0;
        }

        public override string ToString()
        {
            return $"Каждый {this.Interval} месяц {this.DayOfMonth} числа.";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class RelativeMonthlyPattern : IntervalPattern
    {
        public RelativeMonthlyPattern(DateTime startDate, int interval, DayOfTheWeek dayOfTheWeek, DayOfTheWeekIndex dayOfTheWeekIndex)
        {
            this.StartDate = startDate;
            this.Interval = interval;
            this.DayOfTheWeek = dayOfTheWeek;
            this.DayOfTheWeekIndex = dayOfTheWeekIndex;
        }

        public DayOfTheWeek DayOfTheWeek { get; set; }

        public DayOfTheWeekIndex DayOfTheWeekIndex { get; set; }

        public override Interval GetBusyInterval(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
